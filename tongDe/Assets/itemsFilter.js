const urlParams = new URLSearchParams(window.location.search);
const selectedItemCategoryId = urlParams.get('itemCategoryId')
const itemCategorySelect = document.querySelector('#item-category-select');

itemCategorySelect.addEventListener('change', (e) => {
  const itemCategoryId = e.target.value;
  if (itemCategoryId === '-1') return;
  document.querySelector('#filter-form').submit();
})

if (selectedItemCategoryId !== null) {
  var selectedOption = document.querySelector(`#item-category-select option[value="${selectedItemCategoryId}"]`);
  selectedOption.selected = true;
}