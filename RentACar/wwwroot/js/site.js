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
    const priceRangeStart = document.getElementById("PriceRangeStart");
    const priceRangeEnd = document.getElementById("PriceRangeEnd");

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

    // Update hidden inputs
    if (priceRangeStart && priceRangeEnd) {
        priceRangeStart.value = priceMin.value;
        priceRangeEnd.value = priceMax.value;
    }

    // Update slider track
    updateSliderTrack();
}

document.addEventListener("DOMContentLoaded", () => {
    updateSliderTrack();
});
function updateSliderTrack() {
    const priceMin = document.getElementById("priceMin");
    const priceMax = document.getElementById("priceMax");
    const sliderTrack = document.querySelector(".slider-track");

    if (!priceMin || !priceMax || !sliderTrack) {
        return;
    }

    const minPercent = (priceMin.value / priceMin.max) * 100;
    const maxPercent = (priceMax.value / priceMax.max) * 100;

    sliderTrack.style.background = `linear-gradient(to right, #ddd ${minPercent}%, #007bff ${minPercent}%, #007bff ${maxPercent}%, #ddd ${maxPercent}%)`;
}


// Client-side functionality for Car Rental Calendar

document.addEventListener("DOMContentLoaded", function () {
    // References to DOM elements
    const calendarDates = document.getElementById("calendar-dates");
    const yearSelect = document.getElementById("year");
    const monthSelect = document.getElementById("month");
   /* const reservedDates = ["2025-01-28", "2025-01-29"];*/ // Reserved dates from backend
    const selectedDates = []; // Array to store user-selected start and end dates
    let currentSelectedRange = []; // Array to store all dates in the currently selected range
  /*  const pricePerDay = parseFloat("@Model.Car.PricePerDay"); // Price per day of the car*/
    const totalPriceElement = document.getElementById("total-price"); // Element to display total price
    const selectedDatesElement = document.getElementById("selected-dates"); // Element to display selected dates

    if (!calendarDates || !yearSelect || !monthSelect) {
     /*   console.error("Required elements are missing from the DOM.");*/
        return;
    }

    const months = [
        "Януари", "Февруару", "Март", "Април", "Май", "Юни",
        "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември"
    ];

    // Populate the year dropdown (current year + next 5 years)
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year <= currentYear + 5; year++) {
        const option = document.createElement("option");
        option.value = year;
        option.textContent = year;
        yearSelect.appendChild(option);
    }

    // Populate the month dropdown
    months.forEach((month, index) => {
        const option = document.createElement("option");
        option.value = index;
        option.textContent = month;
        monthSelect.appendChild(option);
    });

    // Set default values for year and month dropdowns
    yearSelect.value = currentYear;
    monthSelect.value = new Date().getMonth();

    // Function to render the calendar based on selected year and month
    function renderCalendar() {
        const year = parseInt(yearSelect.value); // Selected year
        const month = parseInt(monthSelect.value); // Selected month
        const firstDay = new Date(year, month, 1).getDay(); // Day of the week the month starts
        const daysInMonth = new Date(year, month + 1, 0).getDate(); // Total days in the selected month

        calendarDates.innerHTML = ""; // Clear existing calendar dates

        // Adjust firstDay to start the week from Monday
        const adjustedFirstDay = (firstDay === 0 ? 6 : firstDay - 1);

        // Add placeholder elements for days before the first day of the month
        for (let i = 0; i < adjustedFirstDay; i++) {
            const placeholder = document.createElement("div");
            placeholder.className = "placeholder"; // Styling for empty spaces
            calendarDates.appendChild(placeholder);
        }

        // Add calendar dates with appropriate styling and behavior
        for (let day = 1; day <= daysInMonth; day++) {
            const date = `${year}-${String(month + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;
            const dayElement = document.createElement("div");
            dayElement.textContent = day;

            // Check if the date is reserved
            if (reservedDates.includes(date)) {
                dayElement.className = "reserved"; // Styling for reserved dates
            } else if (currentSelectedRange.includes(date)) {
                dayElement.className = "selected"; // Styling for dates in the selected range
            } else {
                dayElement.className = "available"; // Styling for available dates
                dayElement.addEventListener("click", function () {
                    handleDateSelection(date, dayElement); // Handle user selection
                });
            }

            calendarDates.appendChild(dayElement); // Add day to the calendar
        }
    }

    // Function to handle user date selection
    function handleDateSelection(date) {
        if (selectedDates.length === 2) {
            // Clear previous selection if two dates are already selected
            selectedDates.length = 0;
            currentSelectedRange = [];
            document.querySelectorAll(".selected").forEach(el => el.classList.remove("selected"));
        }

        // Add the selected date to the array
        selectedDates.push(date);

        if (selectedDates.length === 2) {
            // Handle range selection when two dates are selected
            const startDate = new Date(selectedDates[0]);
            const endDate = new Date(selectedDates[1]);

            // Ensure startDate is always before endDate
            if (startDate > endDate) [startDate, endDate] = [endDate, startDate];

            let allDatesValid = true; // Flag to check if all selected dates are valid
            currentSelectedRange = []; // Reset the current selected range

            // Generate all dates in the range
            for (let d = new Date(startDate); d <= endDate; d.setDate(d.getDate() + 1)) {
                const formattedDate = new Date(d).toISOString().split("T")[0];
                currentSelectedRange.push(formattedDate);

                // Check if any date in the range is reserved
                if (reservedDates.includes(formattedDate)) {
                    allDatesValid = false;
                }
            }

            if (allDatesValid) {
                // Mark all valid dates as selected
                currentSelectedRange.forEach(date => {
                    const el = Array.from(calendarDates.children).find(
                        e => e.textContent == parseInt(date.split("-")[2]) && !e.classList.contains("reserved")
                    );
                    if (el) el.classList.add("selected");
                });
            } else {
                // Clear invalid selection and notify the user
                selectedDates.length = 0;
                currentSelectedRange = [];
                alert("Your selection includes reserved dates. Please select a valid range.");
                document.querySelectorAll(".selected").forEach(el => el.classList.remove("selected"));
            }
        }

        updateSummary(); // Update the summary section
    }

    // Function to update the summary section
    function updateSummary() {
        // Update the text for selected dates in the summary section
        selectedDatesElement.textContent = currentSelectedRange.length ? currentSelectedRange.join(", ") : "None";

        // Filter out valid dates (exclude reserved ones)
        const validDates = currentSelectedRange.filter(date => !reservedDates.includes(date));

        // Calculate the total price
        const totalPrice = (validDates.length * pricePerDay).toFixed(2);
        totalPriceElement.textContent = totalPrice;

        // Retrieve input elements for updating values
        const dateOfRentInput = document.getElementById("DateOfRent");
        const dateOfReturnInput = document.getElementById("DateOfReturn");
        const totalPriceInput = document.getElementById("TotalPrice");

        // Update the inputs with the selected dates and calculated total price
        if (currentSelectedRange.length > 0) {
            dateOfRentInput.value = currentSelectedRange[0]; // First selected date
            dateOfReturnInput.value = currentSelectedRange[currentSelectedRange.length - 1]; // Last selected date
        } else {
            dateOfRentInput.value = ""; // Clear if no selection
            dateOfReturnInput.value = ""; // Clear if no selection
        }

        // Set the total price value
        totalPriceInput.value = totalPrice;
    }

    // Event listeners for year and month dropdown changes
    yearSelect.addEventListener("change", renderCalendar);
    monthSelect.addEventListener("change", renderCalendar);

    // Initial rendering of the calendar
    renderCalendar();
});

// Function to discard the rent
function discardRent() {
    if (confirm("Are you sure you want to discard the rent?")) {
        window.location.href = "/Car"; // Redirect to car listing or another page
    }
}

// Function to confirm the rent
function confirmRent() {
    alert("Rent confirmed!");
    // Implement API call or form submission for rent confirmation
}

