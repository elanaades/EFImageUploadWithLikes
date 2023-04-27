$(() => {

    const id = $("#image-id").val();

    setInterval(() => {
        $.get('/home/getlikes', { id }, likes => {
            $("#likes-count").text(likes);

        })
    }, 1000);
    
    $("#like-button").on('click', function () {
        $.post('/home/updatelikes', { id }, function () {
        })
    })

})