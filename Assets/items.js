const itemTable = document.querySelector('#item-table');
const mainItemSection = document.querySelector('#main-item-section');
const optionBarStatusMap = {
  true: {
    iconClass: 'fa-xmark'
  },
  false: {
    iconClass: 'fa-chevron-left'
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
  const optionStatus = optionBarStatusMap[object.dataset.optionDisplayStatus];

  object.classList = `fa-solid ${optionStatus.iconClass} option-list-open-button px-4 py-3`;

  const optionId = object.id.match(/\d+$/)[0]
  const optionList = document.querySelector(`#option-list-${optionId}`)

  if (object.dataset.optionDisplayStatus === 'true') {
    optionList.classList.replace('hidden', 'flex')
  } else {
    optionList.classList.replace('flex', 'hidden')
  }
}