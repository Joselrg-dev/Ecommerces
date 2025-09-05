// validation-login.js

class FormUtils {
    static validateEmail(value) {
        if (!value) {
            return { isValid: false, message: 'Escribe un correo válido.' };
        }
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            return { isValid: false, message: 'Formato de correo incorrecto.' };
        }
        return { isValid: true };
    }

    static validatePassword(value) {
        if (!value) {
            return { isValid: false, message: 'La contraseña es obligatoria.' };
        }
        if (value.length < 8) {
            return { isValid: false, message: 'Debe tener al menos 8 caracteres.' };
        }
        return { isValid: true };
    }

    static showError(input, message) {
        input.classList.add("is-invalid");
        const feedback = input.closest(".mb-3, .mb-2")?.querySelector(".invalid-feedback");
        if (feedback) feedback.textContent = message;
    }

    static clearError(input) {
        input.classList.remove("is-invalid");
    }

    static setupPasswordToggle(passwordInput, toggleButton) {
        if (toggleButton && passwordInput) {
            toggleButton.addEventListener("click", () => {
                const isPassword = passwordInput.type === "password";
                passwordInput.type = isPassword ? "text" : "password";

                const icon = toggleButton.querySelector("i");
                if (icon) {
                    icon.classList.toggle("fa-eye");
                    icon.classList.toggle("fa-eye-slash");
                }

                passwordInput.focus();
            });
        }
    }

    static showNotification(message, type = 'info') {
        const container = document.querySelector("#loginForm");
        if (!container) return;

        const alert = document.createElement("div");
        alert.className = `alert alert-${type === 'error' ? 'danger' : type} mt-3`;
        alert.textContent = message;

        container.appendChild(alert);
        setTimeout(() => alert.remove(), 3000);
    }
}

// Inicialización del formulario
document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("loginForm");
    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");
    const togglePassword = document.getElementById("togglePassword");

    FormUtils.setupPasswordToggle(passwordInput, togglePassword);

    form.addEventListener("submit", (e) => {
        e.preventDefault();

        let valid = true;

        // Validar email
        const emailCheck = FormUtils.validateEmail(emailInput.value);
        if (!emailCheck.isValid) {
            FormUtils.showError(emailInput, emailCheck.message);
            valid = false;
        } else {
            FormUtils.clearError(emailInput);
        }

        // Validar password
        const passCheck = FormUtils.validatePassword(passwordInput.value);
        if (!passCheck.isValid) {
            FormUtils.showError(passwordInput, passCheck.message);
            valid = false;
        } else {
            FormUtils.clearError(passwordInput);
        }

        if (valid) {
            form.submit(); // 🚀 Ahora sí hace el POST real a Acceso/Index
        }
    });
});
