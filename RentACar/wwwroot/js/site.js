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
};

// Client-side functionality for Admin Panel search and pagination
document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchQuery");
    const usersPerPageSelect = document.getElementById("usersPerPage");
    const clientsTable = document.getElementById("clientsTable");
    const paginationControls = document.getElementById("paginationControls");

    // Set the selected value of the dropdown, if the element exists
    if (usersPerPageSelect) {
        const selectedValue = usersPerPageSelect.getAttribute("data-selected-value");
        if (selectedValue) {
            usersPerPageSelect.value = selectedValue;
        }
    }

    // Debounced fetch function for search
    let searchTimeout;
    if (searchInput) {
        searchInput.addEventListener("input", function () {
            clearTimeout(searchTimeout); // Clear the previous timeout
            searchTimeout = setTimeout(() => {
                fetchClients(1, searchInput.value, usersPerPageSelect ? usersPerPageSelect.value : 3);
            }, 300); // Delay the request by 300ms
        });
    }

    // Handle users per page change
    if (usersPerPageSelect) {
        usersPerPageSelect.addEventListener("change", function () {
            fetchClients(1, searchInput ? searchInput.value : "", usersPerPageSelect.value);
        });
    }

    // Event delegation for pagination buttons
    document.body.addEventListener("click", function (e) {
        if (e.target && e.target.classList.contains("btn-pagination") && !e.target.classList.contains("disabled")) {
            e.preventDefault();
            const page = e.target.getAttribute("data-page");
            fetchClients(page, searchInput ? searchInput.value : "", usersPerPageSelect ? usersPerPageSelect.value : 3);
        }
    });


    // Function to fetch and update the table and pagination via AJAX
    function fetchClients(page, searchQuery, usersPerPage) {
        const url = `/Admin/Index?page=${page}&searchQuery=${encodeURIComponent(searchQuery)}&usersPerPage=${usersPerPage}`;
        fetch(url, { method: "GET" })
            .then((response) => response.text())
            .then((html) => {
                const parser = new DOMParser();
                const newDocument = parser.parseFromString(html, "text/html");

                // Replace the table content
                const newTable = newDocument.querySelector("#clientsTable");
                if (newTable && clientsTable) {
                    clientsTable.innerHTML = newTable.innerHTML;
                }

                // Replace pagination controls
                const newPagination = newDocument.querySelector("#paginationControls");
                if (newPagination && paginationControls) {
                    paginationControls.innerHTML = newPagination.innerHTML;
                }
            })
            .catch((error) => console.error("Error fetching clients:", error));
    }
});

// Price Range Filter Functionality
function updatePriceRange() {
    const priceMin = document.getElementById("priceMin");
    const priceMax = document.getElementById("priceMax");
    const priceMinValue = document.getElementById("priceMinValue");
    const priceMaxValue = document.getElementById("priceMaxValue");
    const sliderTrack = document.querySelector(".slider-track");

    // Prevent sliders from crossing
    if (parseInt(priceMin.value) >= parseInt(priceMax.value)) {
        priceMin.value = parseInt(priceMax.value) - 10;
    }
    if (parseInt(priceMax.value) <= parseInt(priceMin.value)) {
        priceMax.value = parseInt(priceMin.value) + 10;
    }

    // Update displayed values
    priceMinValue.textContent = `$${priceMin.value}`;
    priceMaxValue.textContent = `$${priceMax.value}`;

    // Update slider track
    updateSliderTrack();
}

function updateSliderTrack() {
    const priceMin = document.getElementById("priceMin");
    const priceMax = document.getElementById("priceMax");
    const sliderTrack = document.querySelector(".slider-track");

    const minPercent = (priceMin.value / priceMin.max) * 100;
    const maxPercent = (priceMax.value / priceMax.max) * 100;

    sliderTrack.style.background = `linear-gradient(to right, #ddd ${minPercent}%, #007bff ${minPercent}%, #007bff ${maxPercent}%, #ddd ${maxPercent}%)`;
}

// Initialize slider track
document.addEventListener("DOMContentLoaded", () => {
    updateSliderTrack();
});




