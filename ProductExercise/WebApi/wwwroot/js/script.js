async function fetchAddresses() {
  try {
    const response = await fetch('/api/address');
    if (!response.ok) throw new Error('Erreur de récupération des adresses');
    const addresses = await response.json();
    displayAddresses(addresses);
  } catch (error) {
    showMessage(error.message, 'error');
  }
}

function displayAddresses(addresses) {
  const tableBody = document.getElementById('addressesTableBody');
  tableBody.innerHTML = '';

  addresses.forEach(address => {
    const row = document.createElement('tr');
    row.innerHTML = `
        <td>${address.clientId}</td>
        <td>${address.street}</td>
        <td>${address.zipCode}</td>
        <td>${address.city}</td>
        <td>${address.country}</td>
        <td>
            <button onclick="openEditModal(${address.addressId}, ${address.clientId}, '${address.street}', '${address.zipCode}', '${address.city}', '${address.country}')">Modifier</button>
            <button onclick="deleteAddress(${address.addressId})">Supprimer</button>
        </td>
    `;
    tableBody.appendChild(row);
  });
}

function showMessage(message, type) {
  const messageElement = document.getElementById('message');
  messageElement.textContent = message;
  messageElement.className = `message ${type}`;
  messageElement.style.display = 'block';

  setTimeout(() => {
    messageElement.style.display = 'none';
  }, 5000);
}

async function createAddress() {
  const clientId = parseInt(document.getElementById('clientId').value);
  const street = document.getElementById('street').value;
  const zipCode = document.getElementById('zipCode').value;
  const city = document.getElementById('city').value;
  const country = document.getElementById('country').value;

  const address = { street, zipCode, city, country, clientId };

  try {
    const response = await fetch('/api/address', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(address)
    });

    if (response.ok) {
      showMessage("Adresse créée avec succès!", 'success');
      fetchAddresses();
    } else {
      const errorText = await response.text();
      showMessage("Erreur lors de la création de l'adresse: " + errorText, 'error');
    }
  } catch (error) {
    showMessage("Erreur réseau ou serveur: " + error.message, 'error');
  }
}

function openEditModal(addressId, clientId, street, zipCode, city, country) {
  document.getElementById('editAddressId').value = addressId;
  document.getElementById('editClientId').value = clientId;
  document.getElementById('editStreet').value = street;
  document.getElementById('editZipCode').value = zipCode;
  document.getElementById('editCity').value = city;
  document.getElementById('editCountry').value = country;

  document.getElementById('editModal').style.display = 'block';
}

function closeEditModal() {
  document.getElementById('editModal').style.display = 'none';
}

async function updateAddress() {
  const id = document.getElementById('editAddressId').value;
  const clientId = parseInt(document.getElementById('editClientId').value);
  const street = document.getElementById('editStreet').value;
  const zipCode = document.getElementById('editZipCode').value;
  const city = document.getElementById('editCity').value;
  const country = document.getElementById('editCountry').value;

  const address = { clientId, street, zipCode, city, country };

  try {
      const response = await fetch(`/api/address/${id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(address)
      });

      if (!response.ok) {
          closeEditModal();
          const errorText = await response.text(); 
          showMessage("Erreur lors de la mise à jour de l'adresse: " + errorText, 'error');
          throw new Error(errorText);
      }
      
      showMessage('Adresse mise à jour avec succès!', 'success');

      closeEditModal();
      fetchAddresses();
  } catch (error) {
      showMessage("Erreur réseau ou serveur: " + error.message, 'error');
  }
}


async function deleteAddress(id) {
  try {
    const response = await fetch(`/api/address/${id}`, {
      method: 'DELETE'
    });

    if (response.ok) {
      showMessage('Adresse supprimée avec succès!', 'success');
      fetchAddresses();
    } else {
      throw new Error('Erreur lors de la suppression de l\'adresse');
    }
  } catch (error) {
    showMessage(error.message, 'error');
  }
}

window.onload = function() {
  fetchAddresses();
}
