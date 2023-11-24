function addTagToForm(){
    let index = 1;
    let div = document.getElementById("additional-tags");
    
    let newChild = document.createElement('div');
    newChild.classList.add('form-floating');
    
    let label = document.createElement('label');
    label.setAttribute('for', "Item_Tags_" + index + "_")
    label.innerText = "Nazwa taga";
    
    let input = document.createElement('label');
    input.classList.add('form-control');
    input.setAttribute('required', 'true');
    input.setAttribute('name', "Item.Tags[" + index + "]");
    input.setAttribute('id', "Item_Tags_" + index + "_");
    input.setAttribute('type', "text");

    newChild.insertAdjacentElement('afterend', input);
    newChild.insertAdjacentElement('afterend', label);
    
    div.insertAdjacentElement("afterend", newChild);
}