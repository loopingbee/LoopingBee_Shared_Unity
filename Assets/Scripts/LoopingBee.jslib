mergeInto(LibraryManager.library, {
    gameOver: function (result, score, showcaseDelay) {
        const message = JSON.stringify({
            result: result == 1,
            score,
            showcaseDelay
        });

        console.log(`Game Over: ${message}`);
    },
    purchaseProduct: function (product_id, uuid)
    {
        const message = JSON.stringify({
            action: "purchase",
            data: {
                product_id,
                uuid
            }
        })

        console.log(`Action: ${message}`);
    }
});