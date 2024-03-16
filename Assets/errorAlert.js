import { alertToast } from './izitoast.js';

const errorMessage = document.querySelector('#error-message');

if (errorMessage !== null) {
  alertToast(errorMessage.value);
}