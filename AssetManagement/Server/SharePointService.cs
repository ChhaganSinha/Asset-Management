using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AssetManagement.Dto;
using Azure.Identity;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace AssetManagement.Server
{
    public class SharePointService
    {
        private readonly string tenantId = "cf92019c-152d-42f6-bbcc-0cf96e6b0108";
        private readonly string clientId = "ab34deba-f294-4103-995f-0f93546acb2e";
        private readonly string clientSecret = "GuU8Q~Dtf7iGj-Msa2VwnkozOSEN2vMT~YrHbbJP";
        string siteid = "Sharepoint Demo";
        string root = "https://credentinfotec.sharepoint.com";
        private readonly string siteUrl = "https://credentinfotec.sharepoint.com/sites/intranet";
        string[] scopes = new[] { "https://graph.microsoft.com/.default" };
        GraphServiceClient _graphClient;
        private readonly string listId = "ca2134e5-e964-4553-981c-7d6be9bb9c8e"; //employee Master 
        private readonly string list2Id = "6081f6ff-d7b6-4440-860f-13f54228b576"; //Grant application permission
        public SharePointService()
        {
            var options = new ClientSecretCredentialOptions()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithTenantId(tenantId)
            .Build();
            var authResult = confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync().Result;
            //var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
            var authProvider = new DelegateAuthenticationProvider(req =>
            {
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                return Task.FromResult(0);
            });
            _graphClient = new GraphServiceClient(authProvider);
        }

        public async Task InsertDummyDataIntoListAsync()
        {
            var listId = "42d95848-2f80-4133-bd98-73489f635755"; // ID of the TestEMPM list

            // Create a new ListItem object with dummy data
            var newItem = new Microsoft.Graph.ListItem
            {
                Fields = new FieldValueSet
                {
                    AdditionalData = new Dictionary<string, object>
                    {
                        { "Title", "Dummy Title" },
                        { "Gender", "Male" },
                        { "Designation", "Software Engineer" },
                        { "DOB", new DateTime(1990, 5, 15) },
                        { "Location", "City" },
                        { "Date_x0020_of_x0020_joining", new DateTime(2018, 10, 1) },
                        { "Department", "IT" },
                        { "Status", "Active" },
                        { "EmployeeName", "John Doe" },
                        { "EmployeeID", "EMP001" },
                        { "Email", "john.doe@example.com" },
                        { "ManagerID", "MGR001" },
                        { "ManagerEmail", "manager@example.com" },

                    }
                }
            };

            try
            {
                var createdItem = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items.Request().AddAsync(newItem);
                Console.WriteLine($"Dummy item inserted successfully with ID: {createdItem.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting dummy item: {ex.Message}");
            }
        }
        public async Task GetSitesAsync()
        {
            try
            {
                var site = await _graphClient.Sites[siteUrl].Request().GetAsync();
                Console.WriteLine($"Site Name: {site.Name}, Site ID: {site.Id}, Site URL: {site.WebUrl}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving site: {ex.Message}");
            }
        }

        public async Task GetSiteListsAsync()
        {
            try
            {
                var siteLists = await _graphClient.Sites[siteUrl].Lists.Request().GetAsync();
                foreach (var list in siteLists)
                {
                    Console.WriteLine($"List Name: {list.DisplayName}, List ID: {list.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving lists: {ex.Message}");
            }
        }

        public async Task<SharepointListUpdate> InsertOrUpdateListItemAsync(SharepointListUpdate request)
        {
            try
            {
                // Check if email exists in Azure AD
                bool userExists = await UserExistsAsync(request.Email);
                if (!userExists)
                {
                    await CreateGuestUserAsync(request.Email, request.EmployeeName);
                    Console.WriteLine($"Guest user created with email '{request.Email}'.");
                }

                // Retrieve items from the first list
                //var items = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items
                //    .Request()
                //    .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                //    .Expand("Fields")
                //    .Filter($"fields/AadharNumber eq '{request.AadharNumber}'")
                //    .GetAsync();

                //// Retrieve items from the second list
                //var grantApplicationPermissionItems = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[list2Id].Items
                //    .Request()
                //    .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                //    .Expand("Fields")
                //    .Filter($"fields/AadharNumber eq '{request.AadharNumber}'")
                //    .GetAsync();

                var items = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items
                   .Request()
                   .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                   .Expand("Fields")
                   .Filter($"fields/Email eq '{request.Email}'")
                   .GetAsync();

                if(items == null || items.Count == 0)
                {
                    items = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items
                   .Request()
                   .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                   .Expand("Fields")
                   .Filter($"fields/AadharNumber eq '{request.AadharNumber}'")
                   .GetAsync();
                }

                // Retrieve items from the second list
                var grantApplicationPermissionItems = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[list2Id].Items
                    .Request()
                    .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                    .Expand("Fields")
                    .Filter($"fields/UserEmail eq '{request.Email}'")
                    .GetAsync();

                if (items.Count == 1)
                {
                    var itemToUpdate = items[0];
                    var updatedItem = new ListItem
                    {
                        Fields = new FieldValueSet
                        {
                            AdditionalData = new Dictionary<string, object>
                            {
                                { "Gender", request.Gender },
                                { "AadharNumber", request.AadharNumber },
                                { "Designation", request.Designation },
                                { "DOB", new DateTime(request.DOB.Year == 1 ? 1 : request.DOB.Year, request.DOB.Month, request.DOB.Day) },
                                { "Location", request.Location },
                                { "Date_x0020_of_x0020_joining", new DateTime(request.DateOfJoining.Year == 1 ? 1 : request.DateOfJoining.Year, request.DateOfJoining.Month, request.DateOfJoining.Day) },
                                { "Department", request.Department },
                                { "Status", request.Status },
                                { "EmployeeName", request.EmployeeName },
                                { "EmployeeID", request.EmployeeID },
                                { "Email", request.Email },

                                //{ "ManagerID", request.ManagerID },
                                //{ "ManagerEmail", request.ManagerEmail },

                                { "EMSManagerID", request.EMSManagerID },
                                { "EMSManagerName", request.EMSManagerName },
                                { "EMSManagerEmail", request.EMSManagerEmail },
                                { "LMManagerID", request.LMManagerID },
                                { "LMManagerName", request.LMManagerName },
                                { "LMManagerEmail", request.LMManagerEmail },

                                { "ManagerID", request.EMSManagerID },
                                { "Manager", request.EMSManagerName },
                                { "ManagerEmail", request.EMSManagerEmail }
                            }
                        }
                    };

                    await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items[itemToUpdate.Id]
                        .Request()
                        .UpdateAsync(updatedItem);

                    Console.WriteLine($"Item with email '{request.Email}' updated successfully in listId.");
                }
                else
                {
                    var newItem = new ListItem
                    {
                        Fields = new FieldValueSet
                        {
                            AdditionalData = new Dictionary<string, object>
                            {
                                { "Gender", request.Gender },
                                { "AadharNumber", request.AadharNumber },
                                { "Designation", request.Designation },
                                { "DOB", new DateTime(request.DOB.Year == 1 ? 1 : request.DOB.Year, request.DOB.Month, request.DOB.Day) },
                                { "Location", request.Location },
                                { "Date_x0020_of_x0020_joining", new DateTime(request.DateOfJoining.Year == 1 ? 1 : request.DateOfJoining.Year, request.DateOfJoining.Month, request.DateOfJoining.Day) },
                                { "Department", request.Department },
                                { "Status", request.Status },
                                { "EmployeeName", request.EmployeeName },
                                { "EmployeeID", request.EmployeeID },
                                { "Email", request.Email },

                                //{ "ManagerID", request.ManagerID },
                                //{ "ManagerEmail", request.ManagerEmail },

                                { "EMSManagerID", request.EMSManagerID },
                                { "EMSManagerName", request.EMSManagerName },
                                { "EMSManagerEmail", request.EMSManagerEmail },
                                { "LMManagerID", request.LMManagerID },
                                { "LMManagerName", request.LMManagerName },
                                { "LMManagerEmail", request.LMManagerEmail },

                                { "ManagerID", request.EMSManagerID },
                                { "Manager", request.EMSManagerName },
                                { "ManagerEmail", request.EMSManagerEmail }
                            }
                        }
                    };

                    var createdItem = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items.Request().AddAsync(newItem);
                    Console.WriteLine($"New item inserted with email '{request.Email}' in listId.");
                }

                if (grantApplicationPermissionItems.Count == 1)
                {
                    var grantApplicationPermissionItemToUpdate = grantApplicationPermissionItems[0];
                    var grantApplicationPermissionUpdateItems = new ListItem
                    {
                        Fields = new FieldValueSet
                        {
                            AdditionalData = new Dictionary<string, object>
                            {
                                { "UserEmail", request.Email },
                                { "LM", request.LM },
                                { "EMS", request.EMS },
                                { "CMS", request.CMS },
                                { "AadharNumber", request.AadharNumber },
                                { "SitePermission", request.Portal },
                                { "RunWF", "Yes" }
                            }
                        }
                    };

                    await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[list2Id].Items[grantApplicationPermissionItemToUpdate.Id]
                        .Request()
                        .UpdateAsync(grantApplicationPermissionUpdateItems);

                    Console.WriteLine($"Item with email '{request.Email}' updated successfully in list2Id.");
                }
                else
                {
                    var grantApplicationPermissionNewItem = new ListItem
                    {
                        Fields = new FieldValueSet
                        {
                            AdditionalData = new Dictionary<string, object>
                            {
                                { "UserEmail", request.Email },
                                { "LM", request.LM },
                                { "EMS", request.EMS },
                                { "CMS", request.CMS },
                                { "AadharNumber", request.AadharNumber },
                                { "SitePermission", request.Portal },
                                { "RunWF", "Yes" }
                            }
                        }
                    };

                    var createdGrantApplicationPermissionItem = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[list2Id].Items.Request().AddAsync(grantApplicationPermissionNewItem);
                    Console.WriteLine($"New item inserted with email '{request.Email}' in list2Id.");
                }

               

                return request;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting or updating item: {ex.Message}");
                return null;
            }
        }



        public async Task<SharepointListUpdate> GetListItemByEmail(string email)
        {
            try
            {
                // Retrieve items from the first list
                var items = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[listId].Items
                    .Request()
                    .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                    .Expand("fields")
                    .Filter($"fields/AadharNumber eq '{email}'")
                    .GetAsync();

                // Retrieve items from the second list
                var grantApplicationPermissionItems = await _graphClient.Sites["root"].SiteWithPath("/sites/intranet").Lists[list2Id].Items
                    .Request()
                    .Header("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly")
                    .Expand("fields")
                    .Filter($"fields/AadharNumber eq '{email}'")
                    .GetAsync();

                var sharepointListUpdate = new SharepointListUpdate();

                // Check and retrieve data from the first list
                if (items.Count >= 1)
                {
                    var item = items[0];

                    item.Fields.AdditionalData.TryGetValue("EMSManagerEmail", out var managerEmail);
                    sharepointListUpdate.EMSManagerEmail = managerEmail?.ToString();

                    item.Fields.AdditionalData.TryGetValue("LMManagerEmail", out var lmManagerEmail);
                    sharepointListUpdate.LMManagerEmail = lmManagerEmail?.ToString();
                }

                // Check and retrieve data from the second list
                if (grantApplicationPermissionItems.Count >= 1)
                {
                    var grantApplicationPermissionItem = grantApplicationPermissionItems[0];
                    sharepointListUpdate.Portal = bool.TryParse(grantApplicationPermissionItem.Fields.AdditionalData["SitePermission"]?.ToString(), out bool sp) ? sp : false;
                    sharepointListUpdate.LM = bool.TryParse(grantApplicationPermissionItem.Fields.AdditionalData["LM"]?.ToString(), out bool lm) ? lm : false;
                    sharepointListUpdate.EMS = bool.TryParse(grantApplicationPermissionItem.Fields.AdditionalData["EMS"]?.ToString(), out bool ems) ? ems : false;
                    sharepointListUpdate.CMS = bool.TryParse(grantApplicationPermissionItem.Fields.AdditionalData["CMS"]?.ToString(), out bool cms) ? cms : false;

                }

                return sharepointListUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving item: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                var user = await _graphClient.Users[email].Request().GetAsync();
                return user != null;
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
                throw;
            }
        }


        private async Task CreateGuestUserAsync(string email, string displayName)
        {
            var invitation = new Invitation
            {
                InvitedUserEmailAddress = email,
                InviteRedirectUrl = "https://credentinfotec.sharepoint.com/sites/intranet/Pages/NewPage.aspx",
                InvitedUserDisplayName = displayName,
                SendInvitationMessage = true
            };

            await _graphClient.Invitations.Request().AddAsync(invitation);
        }

    }
}
