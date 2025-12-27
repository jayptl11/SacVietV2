# SacViet Logo Implementation Guide

This implementation has converted the text logos to image logos throughout the website. You need to provide PNG images with transparent backgrounds for the following files:

## Required Logo Files

### 1. Large Logo (Main Homepage)
- **File:** `sacviet-logo-large.png`
- **Recommended size:** 500px width x 120px height (max)
- **Format:** PNG with transparent background
- **Usage:** Large logo section below navbar on homepage
- **Design:** Full logo with best quality, used as main brand display

### 2. Compact Navbar Logo
- **File:** `sacviet-logo-compact-white.png`
- **Recommended size:** 140px width x 32px height (max)
- **Format:** PNG with transparent background
- **Color:** WHITE version (displays on red navbar)
- **Usage:** Appears in navbar when user scrolls down
- **Design:** Compact/simplified logo version suitable for small spaces

### 3. Mobile Menu Logo
- **File:** `sacviet-logo-mobile-white.png`
- **Recommended size:** 100px width x 24px height (max)
- **Format:** PNG with transparent background
- **Color:** WHITE version (displays on red mobile menu header)
- **Usage:** Mobile offcanvas menu header
- **Design:** Very compact version, minimal details

### 4. Footer Logo
- **File:** `sacviet-logo-footer.png`
- **Recommended size:** 200px width x 60px height (max)
- **Format:** PNG with transparent background
- **Usage:** Footer section
- **Design:** Medium-sized version, readable but not dominant

## Logo Design Guidelines

1. **Transparency:** All logos must have transparent backgrounds (PNG format)
2. **Quality:** Use high-resolution images for crisp display on all devices
3. **Consistency:** Maintain brand consistency across all logo variations
4. **Color Variants:**
   - White versions for red backgrounds (navbar, mobile menu)
   - Regular color versions for light backgrounds (footer, main logo)

## Installation Steps

1. Create your logo images according to the specifications above
2. Save them in the `wwwroot/images/` directory with the exact filenames listed
3. Remove the placeholder `.txt` files
4. Test on different screen sizes to ensure proper display
5. Verify the logos display correctly when scrolling (navbar logo animation)

## Fallback Behavior

If any logo image fails to load, the browser will show a broken image icon or the alt text. Consider adding CSS fallback styles or keeping the original text logo as a backup option.

## Responsive Behavior

The logos are responsive and will scale appropriately on different screen sizes:
- Desktop: Full size logos
- Tablet: Medium scaling
- Mobile: Smallest versions optimized for mobile screens

## Browser Compatibility

The implementation uses modern CSS properties but maintains compatibility with:
- Chrome/Edge/Firefox (latest versions)
- Safari (latest versions)
- Mobile browsers (iOS Safari, Chrome Mobile)

## Testing Checklist

- [ ] Large logo displays on homepage
- [ ] Compact logo appears when scrolling down
- [ ] Mobile menu logo shows correctly
- [ ] Footer logo displays properly
- [ ] All logos scale correctly on mobile devices
- [ ] Logo hover effects work smoothly
- [ ] Images load quickly and appear crisp