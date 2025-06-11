<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>
<script>
    const conn = new signalR.HubConnectionBuilder()
        .withUrl("/notifyHub")
        .build();

    conn.on("DataChanged", function () {
        location.reload();
    });

    conn.start().catch(console.error);
</script>
