import { pinyinConverter } from "./pinyinConverter";
import { alertToast, successToast } from './izitoast.js';

const pinyinConvertButton = document.querySelector('#convert-button')
const itemAliasList = document.querySelector("#item-alias-list");
const itemAliasCreateButton = document.querySelector("#item-alias-create-button");
const itemAliasDeleteButton = document.querySelector("#item-alias-delete-button");
const itemAliasNameInput = document.querySelector("#item-alias-name");

//拼音轉換
pinyinConvertButton.addEventListener('click', () => {
  const nameInput = document.querySelector('#Name')
  const pinyinInput = document.querySelector('#PinyIn');

  pinyinInput.value = pinyinConverter(nameInput.value);
})

//itemAlias create
itemAliasCreateButton.addEventListener('click', async (e) => {
  e.preventDefault();
  const itemAliasNameValue = itemAliasNameInput.value.trim();
  const itemId = document.querySelector("#item-id").value;

  if (itemAliasNameValue === '') return;

  const itemAlias = {
    Name: itemAliasNameValue,
    ItemId: itemId
  };

  const createResponse = await createItemAlias(`${import.meta.env.VITE_BASE_URL}/api/Item/${itemId}/ItemAlias/Create`, itemAlias);

  if (createResponse) {
    successToast(`Item alias '${createResponse.name}' created`);
    itemAliasNameInput.value = '';

    const itemAliasListItem = `<span class="bg-blue-100 text-blue-800 text-lg font-medium me-2 px-2.5 py-0.5 rounded dark:bg-blue-900 dark:text-blue-300" data-item-alias-id="${createResponse.id}">${createResponse.name}</span>`
    itemAliasList.insertAdjacentHTML('beforeend', itemAliasListItem);
  }
})

async function createItemAlias(url = "", data = {}) {
  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(data),
    });

    const responseData = await response.json();

    if (response.ok) {
      return responseData;
    } else {
      throw responseData;
    }
  } catch (error) {
    displayErrors(error.errors);

  }
}

function displayErrors(errors) {
  for (const key in errors) {
    if (errors.hasOwnProperty(key)) {
      const errorMessage = `${key}: ${errors[key].join()}`;
      alertToast(errorMessage);
    }
  }
}

itemAliasList.addEventListener('click', (e) => {
  if (e.target && e.target.matches('span')) {
    const itemAliasName = e.target.innerText.trim();
    const itemAliasId = e.target.dataset.itemAliasId;

    itemAliasNameInput.value = itemAliasName
    document.querySelector("#item-alias-id").value = itemAliasId;
  }
})

itemAliasDeleteButton.addEventListener('click', async (e) => {
  e.preventDefault();
  const itemAliasNameValue = itemAliasNameInput.value.trim();
  const itemAliasIdValue = document.querySelector("#item-alias-id").value;
  if (itemAliasNameValue === '') return;

  const isConfirm = confirm(`Are you sure you want to delete ${itemAliasNameValue}?`);

  if (!isConfirm) return;

  const deleteResponse = await deleteItemAlias(`${import.meta.env.VITE_BASE_URL}/api/ItemAlias/Delete/${itemAliasIdValue}`);

  if (deleteResponse) {
    successToast(`Item alias '${deleteResponse.name}' deleted`);
    itemAliasNameInput.value = '';
    const elementToDelete = document.querySelector(`[data-item-alias-id="${itemAliasIdValue}"]`);
    elementToDelete.remove();
  }
})

async function deleteItemAlias(url = "", data = {}) {
  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(data),
    });

    const responseData = await response.json();

    if (response.ok) {
      return responseData;
    } else {
      throw responseData;
    }
  } catch (error) {
    displayErrors(error.errors);

  }
}