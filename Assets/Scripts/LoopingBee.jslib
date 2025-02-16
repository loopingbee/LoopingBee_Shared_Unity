mergeInto(LibraryManager.library, {
    gameOver: function (result, score, allowContinue, showcaseDelay) {
        const message = JSON.stringify({
            result: result == 1,
            score,
            showcaseDelay,
            allowContinue: allowContinue == 1
        });

        console.log(`Game Over: ${message}`);
    },
    purchaseProduct: function (product_id, uuid)
    {
        const message = JSON.stringify({
            action: "purchase",
            data: {
                product_id: UTF8ToString(product_id),
                uuid: UTF8ToString(uuid)
            }
        })

        console.log(`Action: ${message}`);
    }
});