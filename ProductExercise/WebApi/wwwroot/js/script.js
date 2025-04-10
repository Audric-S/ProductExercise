// Fonction pour récupérer toutes les adresses
async function fetchAddresses() {
    try {
        const response = await fetch('/api/address');
        if (!response.ok) throw new Error('Erreur de récupération des adresses');
        const addresses = await response.json();
        displayAddresses(addresses);
    } catch (error) {
        alert(error.message);
    }
}

// Fonction pour afficher les adresses dans le tableau
function displayAddresses(addresses) {
    const tableBody = document.getElementById('addressesTableBody');
    tableBody.innerHTML = '';  // Vide le tableau avant d'ajouter les nouvelles lignes

    addresses.forEach(address => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${address.street}</td>
            <td>${address.zipCode}</td>
            <td>${address.city}</td>
            <td>${address.country}</td>
            <td>
                <button onclick="editAddress(${address.addressId})">Modifier</button>
                <button onclick="deleteAddress(${address.addressId})">Supprimer</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

// Fonction pour créer une nouvelle adresse
async function createAddress() {
    const street = document.getElementById('street').value;
    const zipCode = document.getElementById('zipCode').value;
    const city = document.getElementById('city').value;
    const country = document.getElementById('country').value;

    const address = { street, zipCode, city, country };

    try {
        const response = await fetch('/api/address', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(address)
        });

        if (!response.ok) throw new Error('Erreur lors de la création de l\'adresse');
        alert('Adresse créée avec succès!');
        fetchAddresses();  // Recharger la liste des adresses
    } catch (error) {
        alert(error.message);
    }
}

// Fonction pour éditer une adresse
async function editAddress(id) {
    const street = prompt("Nouvelle rue :");
    const zipCode = prompt("Nouveau code postal :");
    const city = prompt("Nouvelle ville :");
    const country = prompt("Nouveau pays :");

    const address = { street, zipCode, city, country };

    try {
        const response = await fetch(`/api/address/${id}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(address)
        });

        if (!response.ok) throw new Error('Erreur lors de la mise à jour de l\'adresse');
        alert('Adresse mise à jour avec succès!');
        fetchAddresses();  // Recharger la liste des adresses
    } catch (error) {
        alert(error.message);
    }
}

async function deleteAddress(id) {
    try {
        const response = await fetch(`/api/address/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Erreur lors de la suppression de l\'adresse');
        alert('Adresse supprimée avec succès!');
        fetchAddresses();  // Recharger la liste des adresses
    } catch (error) {
        alert(error.message);
    }
}

window.onload = function() {
    console.log('toto')
    fetchAddresses();
};
