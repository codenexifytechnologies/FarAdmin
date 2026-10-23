// Shared jQuery Validate rules for the admin forms (Register Company, Register Vendor, Update Vendor).
// Load after jquery.validate.min.js. "this.optional" lets optional fields stay empty.

$.validator.addMethod("tenDigitMobile", function (value, element) {
    return this.optional(element) || /^\d{10}$/.test(value);
}, "Mobile number must be exactly 10 digits");

$.validator.addMethod("validEmail", function (value, element) {
    return this.optional(element) || /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(value);
}, "Please enter a valid email address");

$.validator.addMethod("lettersOnly", function (value, element) {
    return this.optional(element) || /^[a-zA-Z ]+$/.test(value.trim());
}, "Only letters and spaces are allowed");

$.validator.addMethod("notBlank", function (value, element) {
    return value.trim().length > 0;
}, "This field is required");

$.validator.addMethod("noSpaces", function (value, element) {
    return this.optional(element) || !/\s/.test(value);
}, "Spaces are not allowed");

// Error display shared by all forms: red text under the field, red outline on the field
window.formValidationDisplay = {
    errorElement: 'p',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback d-block');
        element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    }
};
