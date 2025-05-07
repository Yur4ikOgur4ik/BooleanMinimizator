document.addEventListener("DOMContentLoaded", () => {
    let lastHoveredElement = null;
    let lastFocusedElement = null;

    // Сохраняем элемент, на который навели мышку
    document.body.addEventListener("mouseover", (e) => {
        if (e.target.dataset.help) {
            lastHoveredElement = e.target;
        }
    });

    // Убираем, когда мышка уходит
    document.body.addEventListener("mouseout", (e) => {
        if (lastHoveredElement === e.target) {
            lastHoveredElement = null;
        }
    });

    // Сохраняем элемент, на котором фокус (например, input)
    document.body.addEventListener("focusin", (e) => {
        if (e.target.dataset.help) {
            lastFocusedElement = e.target;
        }
    });

    // Убираем фокус
    document.body.addEventListener("focusout", (e) => {
        if (lastFocusedElement === e.target) {
            lastFocusedElement = null;
        }
    });

    // Обработка нажатия F1
    document.addEventListener("keydown", (e) => {
        if (e.key === "F1") {
            e.preventDefault();

            let helpText = "";

            // Сначала проверим фокус, потом ховер
            if (lastFocusedElement?.dataset.help) {
                helpText = lastFocusedElement.dataset.help;
            } else if (lastHoveredElement?.dataset.help) {
                helpText = lastHoveredElement.dataset.help;
            }

            // Показать справку
            if (helpText) {
                alert("Справка: " + helpText);
            }
        }
    });
});

document.addEventListener("DOMContentLoaded", () => {
    const helpButton = document.getElementById("helpButton");

    if (helpButton) {
        helpButton.addEventListener("click", () => {
            // Открываем справочный документ в новой вкладке
            window.open("/UserHelp/vvedenie.htm", "_blank");
        });
    }
});

