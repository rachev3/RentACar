// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleDropdown(menuId) {
    const dropdown = document.getElementById(menuId);

    // Toggle the display of the dropdown menu
    if (dropdown.style.display === "block") {
        dropdown.style.display = "none";
    } else {
        dropdown.style.display = "block";
    }

    // Close the dropdown when clicking outside
    document.addEventListener("click", function (event) {
        const clickedElement = event.target;
        if (!clickedElement.closest(`#${menuId}`) && !clickedElement.closest(`.dropdown`)) {
            dropdown.style.display = "none";
        }
    }, { once: true });
}

