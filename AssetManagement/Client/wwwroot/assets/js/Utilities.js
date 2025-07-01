window.blazorCloseTab = function () {
    window.close();
};

function downloadFile(fileName, mimeType, content) {
    const blob = new Blob([content], { type: mimeType });

    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = fileName;

    // Append the anchor element to the body
    document.body.appendChild(link);

    // Programmatically trigger the download
    link.click();

    // Clean up resources
    document.body.removeChild(link);
    URL.revokeObjectURL(link.href);
}


window.MoveElementToClass = (fromElement, toClass) => {
    var felement = document.getElementById(fromElement);
    var telement = document.getElementsByClassName(toClass);
    if (!felement || !telement) {
        console.warn('MoveElement: element was not found');
        return false;
    }
    telement[0].appendChild(felement);
    return true;
}

function printDiv(divId) {
    var printContent = document.getElementById(divId).outerHTML;
    var newWin = window.open('', '_blank');
    newWin.document.write('<html><head><title>Print</title></head><body>');

    // Add some additional styling for better print format
    newWin.document.write('<style>body { font-size: 12pt; } @page { size: auto; margin: 10mm; } </style>');

    newWin.document.write(printContent);
    newWin.document.write('</body></html>');
    newWin.document.close();

    // Wait for the content to load before printing
    newWin.onload = function () {
        newWin.print();
        newWin.close();
    };
}


// Function to download an image  employee page
function downloadImage(imageUrl) {
    // Create a hidden anchor element
    var a = document.createElement('a');
    a.href = imageUrl;
    a.download = 'image.jpg'; // You can customize the downloaded file name
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

// Function to disable body scrolling
function disableBodyScroll() {
    // Add custom CSS or JavaScript logic to disable body scrolling as needed
    // For example, you can add "overflow: hidden;" to the body's style.
    document.body.style.overflow = 'hidden';
}

// Function to enable body scrolling
function enableBodyScroll() {
    // Restore the body's default overflow property to enable scrolling.
    document.body.style.overflow = 'auto';
}


//full scren view
function openFullscreen(id) {
    elem = document.getElementById(id);
    if (!document.fullscreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {
        if (elem.requestFullscreen) {
            elem.requestFullscreen();
        } else if (elem.webkitRequestFullscreen) { // Safari
            elem.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) { // IE11
            elem.msRequestFullscreen();
        }
        document.getElementById("popupScreen").className = "oi oi-fullscreen-exit";
        document.getElementById("fullScreenPopupId").style.height="100vh";
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { // Safari
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { // IE11
            document.msExitFullscreen();
        }
        document.getElementById("popupScreen").className = "oi oi-fullscreen-enter";
        document.getElementById("fullScreenPopupId").style.height = "inherit"; 
    }
}
