const itemTable = document.querySelector('#item-table');
const mainItemSection = document.querySelector('#main-item-section');
const optionBarStatusMap = {
  true: {
    iconClass: 'fa-xmark'
    , visibilityClass: 'hidden'
  },
  false: {
    iconClass: 'fa-chevron-down',
    visibilityClass: 'hidden'
  }
};

itemTable.addEventListener('click', (e) => {
  if (e.target.classList.contains('option-list-open-button')) {
    setStatus(e.target);
    setOption(e.target);
  }
})

document.addEventListener('click', (e) => {
  if (e.target.classList.contains('option-list-open-button')) return;
  const itemOptionButtons = document.querySelectorAll('.option-list-open-button');
  itemOptionButtons.forEach((itemOptionButton) => {
    if (itemOptionButton.dataset.optionDisplayStatus === 'false') return;
    setStatus(itemOptionButton);
    setOption(itemOptionButton);
  })
})

function setStatus(object) {
  const currentStatus = object.dataset.optionDisplayStatus === 'true';
  object.dataset.optionDisplayStatus = !currentStatus;
}

function setOption(object) {
  const optionStaus = optionBarStatusMap[object.dataset.optionDisplayStatus];

  object.classList = `fa-solid ${optionStaus.iconClass} option-list-open-button px-4 py-2`;

  const optionId = object.id.match(/\d+$/)[0]
  const optionList = document.querySelector(`#option-list-${optionId}`)

  if (object.dataset.optionDisplayStatus === 'true') {
    optionList.classList.remove(optionStaus.visibilityClass)
  } else {
    optionList.classList.add(optionStaus.visibilityClass)
  }
}