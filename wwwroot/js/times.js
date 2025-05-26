
setInterval(() => {
    const now = new Date();
    const time = `${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
    document.getElementById('dateInput').value = time; // Set the value of the input field to the current time
}, 1000);

setInterval(() => {
    const now = new Date();
    const time = `${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
    document.getElementById('homeTime').textContent = time;
}, 1000);
