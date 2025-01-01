const dropdown = document.getElementById('drugDropdown');
const additionalDropdowns = document.getElementById('additionalDropdowns');
const ndcDropdown = document.getElementById('ndcDropdown');
const insuranceDropdown = document.getElementById('insuranceDropdown');
const detailsCard = document.getElementById('drugDetails');
const loading = document.getElementById('loading');
const error = document.getElementById('error');
const myTable = document.getElementById('myTable');


// Object to track selected values
const selectedValues = {
    id: null,
    drugName: null,
    ndc: null,
    insurance: null
};

const toggleLoading = (show) => {
    loading.classList.toggle('d-none', !show);
};

// Function to populate dropdowns
const populateInsuranceDropdown = (dropdown, items, defaultText) => {
    if (!items || items.length === 0) {
        dropdown.innerHTML = ''; // Clear existing options
        const defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = 'No data available';
        dropdown.appendChild(defaultOption);
        return;
    }
    
    dropdown.innerHTML = ''; // Clear existing options
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.textContent = defaultText;
    dropdown.appendChild(defaultOption);

    //drop empty values
    items = items.filter(item => 
        item !== null && 
        item !== undefined && 
        (typeof item !== 'string' || item.trim() !== '')
    );

    selectedValues.insurance = items[0] || null;
    items.forEach(item => {
        const option = document.createElement('option');
        option.value = item;
        option.textContent = item;
        dropdown.appendChild(option);
    });
};

const populateNDCDropdown = (dropdown, items, defaultText) => {
    if (!items || items.length === 0) {
        dropdown.innerHTML = ''; // Clear existing options
        const defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = 'No data available';
        dropdown.appendChild(defaultOption);
        return;
    }
    
    dropdown.innerHTML = ''; // Clear existing options
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.textContent = defaultText;
    dropdown.appendChild(defaultOption);
    
    // convert to string
    items = items.map(item => item.ndc);
    selectedValues.ndc  = items[0].replace(/-/g, "") || null;
    console.log(selectedValues.ndc);

    
    items.forEach(item => {
        const option = document.createElement('option');
        option.value = item;
        option.textContent = item;
        dropdown.appendChild(option);
    });
  
    

};

// Fetch drug details and populate additional dropdowns
const fetchDrugDetailsById = async (drugID) => {
    toggleLoading(true);
    //mytable.classList.remove('d-none');


    try {
        const response = await fetch(`https://localhost:7189/api/drugs/${drugID}`);
        if (!response.ok) throw new Error('Failed to fetch drug details');
        const drug = await response.json();
        const drugDetails = drug.uniqueNDCs;
        populateNDCDropdown(ndcDropdown, drug.uniqueNDCs  , '-- Select an NDC --');
        populateInsuranceDropdown(insuranceDropdown, drug.insurances , '-- Select Insurance --');
        additionalDropdowns.classList.remove('d-none');
        // ------------------------------------Poulate drug details--------------------------------
        console.log(selectedValues);
        fetchDrugDataWithSearch(selectedValues);


    } catch (err) {
        alert('Failed to load drug details. Please try again.');
    } finally {
        toggleLoading(false);
    }
};


// ---------------------------------------------Fetch drugs and populate main dropdown-----------------------
const fetchAllDrugs = async () => {
    toggleLoading(true);
    error.classList.add('d-none');
    try {
        const response = await fetch('https://localhost:7189/api/drugs');
        if (!response.ok) throw new Error('Failed to fetch drugs');
        const drugs = await response.json();
        
        drugs.forEach(drug => {
            const option = document.createElement('option');
            option.value = drug.drugID;
            option.textContent = drug.drugName;
            dropdown.appendChild(option);
        });


    } catch (err) {
        console.error(err);
        error.classList.remove('d-none');
    } finally {
        toggleLoading(false);
    }
};


const fetchDrugDataWithSearch = async (searchParams = {}) => {
    const baseUrl = "https://localhost:7189/api/drugs/search";

    try {
        // Build URL with query parameters
        const queryParams = new URLSearchParams(
            Object.entries(searchParams).filter(([_, value]) => value !== null && value !== undefined)
        );
        const fullUrl = queryParams.toString() ? `${baseUrl}?${queryParams}` : baseUrl;

        // Fetch data
        const response = await fetch(fullUrl);
        if (response.status == 404) throw new Error(`HTTP error! Status: ${response.status}`);

        const results = await response.json();

        // f the UI with the results
        renderResults(results);
    } catch (error) {
        console.error("Error fetching data:", error.message);
    }
};

/**
 * Renders the results dynamically into the results container.
 * @param {Array} results - Array of result objects.
 */
const renderResults = (results) => {
    console.log(results);
    const tableBody = document.getElementById("resultsTableBody");
    tableBody.innerHTML = ""; // Clear previous entries

    if (results.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="8" class="text-center">No results found</td></tr>`;
        return;
    }

    results.forEach((result) => {
        const row = `
        <tr>
                <td>${new Date(result.date).toLocaleDateString() || "N/A"}</td>
                <td>${result.script || "N/A"}</td>
                <td>${result.drugName || "Unknown"}</td>
                <td>${result.ndc || "N/A"}</td>
                <td>${result.prescriber || "N/A"}</td>
                <td>${result.qty || "N/A"}</td>
                <td>${result.insPay || "N/A"}</td>
                <td>${result.patPay || "N/A"}</td>
                <td>${result.class || "N/A"}</td>
                <td>${result.net || "N/A"}</td>

            </tr>
        `;
        tableBody.insertAdjacentHTML("beforeend", row);
    });
};




//=============================================

// Event listener for main dropdown
dropdown.addEventListener('change', () => {
    const drugID = dropdown.value;

    if (!drugID) {
        //detailsCard.classList.add('d-none');
        additionalDropdowns.classList.add('d-none');
        return;
    }
    //selectedValues.drugName = dropdown.options[dropdown.selectedIndex].text; 
    selectedValues.id = drugID;
    fetchDrugDetailsById(drugID);
});


ndcDropdown.addEventListener('change', (e) => {
    selectedValues.ndc = e.target.value || null;
    console.log(selectedValues);
    var rsult =fetchDrugDataWithSearch(selectedValues);
    console.log(rsult);
});

insuranceDropdown.addEventListener('change', (e) => {
    selectedValues.insurance = e.target.value || null;
    console.log(selectedValues);
    var rsult =fetchDrugDataWithSearch(selectedValues);
    console.log(rsult);
});





// Initialize dropdowns
fetchAllDrugs();
