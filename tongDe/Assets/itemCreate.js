import { pinyinConverter } from "./pinyinConverter";

const pinyinConvertButton = document.querySelector('#convert-button')
pinyinConvertButton.addEventListener('click', () => {
  const nameInput = document.querySelector('#Name')
  const pinyinInput = document.querySelector('#PinyIn');

  pinyinInput.value = pinyinConverter(nameInput.value);
})