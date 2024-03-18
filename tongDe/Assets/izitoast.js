import 'izitoast/dist/css/iziToast.min.css';
import iziToast from 'izitoast/dist/js/iziToast.min.js';

export function alertToast(message) {
  iziToast.show({
    title: 'Error',
    color: 'red',
    close: true,
    position: 'topCenter',
    message: message
  });
}

export function noticeToast(message) {
  iziToast.show({
    title: 'Notice',
    color: 'blue',
    close: true,
    position: 'topCenter',
    message: message
  });
}

export function warningToast(message) {
  iziToast.show({
    title: 'Warning',
    color: 'yellow',
    close: true,
    position: 'topCenter',
    message: message
  });
}

export function successToast(message) {
  iziToast.show({
    title: 'Success',
    color: 'green',
    close: true,
    position: 'topCenter',
    message: message
  });
}
