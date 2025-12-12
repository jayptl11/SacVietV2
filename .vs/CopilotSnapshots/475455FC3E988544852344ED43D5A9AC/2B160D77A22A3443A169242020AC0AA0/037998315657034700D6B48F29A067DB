// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// SacViet - Enhanced User Experience JavaScript

(function () {
    'use strict';

    // ============================================
    // Smooth Dropdown Menu Enhancement
    // ============================================
    const dropdownMenus = document.querySelectorAll('.category-menu .dropdown');
    
    dropdownMenus.forEach(dropdown => {
        const toggle = dropdown.querySelector('.dropdown-toggle');
        const menu = dropdown.querySelector('.dropdown-menu');
        
        if (toggle && menu) {
            let timeoutId;
            
            // Show dropdown on hover
            dropdown.addEventListener('mouseenter', function () {
                clearTimeout(timeoutId);
                toggle.classList.add('show');
                menu.classList.add('show');
                toggle.setAttribute('aria-expanded', 'true');
                
                // Add stagger animation to menu items
                const items = menu.querySelectorAll('.dropdown-item');
                items.forEach((item, index) => {
                    item.style.animation = `fadeInUp 0.3s ease-out ${index * 0.05}s both`;
                });
            });
            
            // Hide dropdown with delay
            dropdown.addEventListener('mouseleave', function () {
                timeoutId = setTimeout(() => {
                    toggle.classList.remove('show');
                    menu.classList.remove('show');
                    toggle.setAttribute('aria-expanded', 'false');
                }, 200);
            });
        }
    });

    // ============================================
    // Mobile Offcanvas Smooth Collapse
    // ============================================
    const offcanvasCollapses = document.querySelectorAll('.offcanvas-body [data-bs-toggle="collapse"]');
    
    offcanvasCollapses.forEach(collapseBtn => {
        collapseBtn.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            const targetCollapse = document.querySelector(targetId);
            
            if (targetCollapse) {
                // Bootstrap handles the collapse, we just add smooth animation
                targetCollapse.addEventListener('show.bs.collapse', function () {
                    this.style.maxHeight = this.scrollHeight + 'px';
                }, { once: true });
                
                targetCollapse.addEventListener('shown.bs.collapse', function () {
                    this.style.maxHeight = 'none';
                }, { once: true });
                
                targetCollapse.addEventListener('hide.bs.collapse', function () {
                    this.style.maxHeight = this.scrollHeight + 'px';
                    setTimeout(() => {
                        this.style.maxHeight = '0';
                    }, 10);
                }, { once: true });
            }
        });
    });

    // ============================================
    // Smooth Scroll to Top Button
    // ============================================
    const scrollToTopBtn = document.createElement('button');
    scrollToTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    scrollToTopBtn.className = 'scroll-to-top';
    scrollToTopBtn.setAttribute('aria-label', 'Cuộn lên đầu trang');
    document.body.appendChild(scrollToTopBtn);

    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            scrollToTopBtn.classList.add('show');
        } else {
            scrollToTopBtn.classList.remove('show');
        }
    });

    scrollToTopBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    // ============================================
    // Smooth Scroll for Category Links on Homepage
    // ============================================
    function smoothScrollToCategory(targetId, highlightDuration = 2000) {
        const targetElement = document.getElementById(targetId);
        if (!targetElement) return false;

        // Tính toán offset cho navbar và category menu
        const navbar = document.getElementById('mainNavbar');
        const categoryMenu = document.getElementById('categoryMenu');
        const navbarHeight = navbar ? navbar.offsetHeight : 0;
        const categoryMenuHeight = categoryMenu ? categoryMenu.offsetHeight : 0;
        const offset = navbarHeight + categoryMenuHeight + 20; // thêm 20px padding

        const targetPosition = targetElement.getBoundingClientRect().top + window.pageYOffset - offset;

        window.scrollTo({
            top: targetPosition,
            behavior: 'smooth'
        });

        // Highlight effect cho section
        targetElement.classList.add('highlight-section');
        setTimeout(() => {
            targetElement.classList.remove('highlight-section');
        }, highlightDuration);

        return true;
    }

    // Xử lý click cho category links
    document.addEventListener('click', function (e) {
        const link = e.target.closest('.category-home-link');
        if (!link) return;

        const href = link.getAttribute('href');
        if (!href) return;

        const hashIndex = href.indexOf('#');
        if (hashIndex === -1) return;

        const hash = href.substring(hashIndex + 1);
        const isHomePage = window.location.pathname === '/Home/Index' || 
                          window.location.pathname === '/' || 
                          window.location.pathname === '';

        // Nếu đang ở trang chủ, scroll đến section
        if (isHomePage) {
            e.preventDefault();
            if (smoothScrollToCategory(hash)) {
                // Update URL without page reload
                history.pushState(null, null, '#' + hash);
            }
        }
        // Nếu không, để link navigate bình thường (sẽ chuyển đến trang chủ với hash)
    });

    // Xử lý khi load trang với hash trong URL
    window.addEventListener('load', function () {
        if (window.location.hash) {
            const hash = window.location.hash.substring(1);
            setTimeout(() => {
                smoothScrollToCategory(hash);
            }, 300); // Delay để đảm bảo page đã render xong
        }
    });

    // ============================================
    // Add animation classes when elements come into view
    // ============================================
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('fade-in-up');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Observe elements with animation class
    document.addEventListener('DOMContentLoaded', function () {
        const animatedElements = document.querySelectorAll('.news-card-home, .timeline-item, .sidebar-news-item, .secondary-card');
        animatedElements.forEach(el => observer.observe(el));
    });

    // ============================================
    // Reading Progress Bar
    // ============================================
    if (document.querySelector('.article-content')) {
        const progressBar = document.createElement('div');
        progressBar.className = 'reading-progress';
        document.body.appendChild(progressBar);

        window.addEventListener('scroll', function () {
            const winScroll = document.body.scrollTop || document.documentElement.scrollTop;
            const height = document.documentElement.scrollHeight - document.documentElement.clientHeight;
            const scrolled = (winScroll / height) * 100;
            progressBar.style.width = scrolled + '%';
        });
    }

    // ============================================
    // Search Input Enhancement
    // ============================================
    const searchInputs = document.querySelectorAll('input[type="text"][name="keyword"]');
    searchInputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', function () {
            if (!this.value) {
                this.parentElement.classList.remove('focused');
            }
        });
    });

    // ============================================
    // Copy Link to Clipboard
    // ============================================
    const copyLinkButtons = document.querySelectorAll('.btn[href="#"]');
    copyLinkButtons.forEach(btn => {
        const icon = btn.querySelector('.fa-link');
        if (icon) {
            btn.addEventListener('click', function (e) {
                e.preventDefault();
                const url = window.location.href;
                
                if (navigator.clipboard) {
                    navigator.clipboard.writeText(url).then(() => {
                        showToast('Đã sao chép liên kết!');
                    });
                } else {
                    // Fallback for older browsers
                    const textArea = document.createElement('textarea');
                    textArea.value = url;
                    textArea.style.position = 'fixed';
                    textArea.style.left = '-999999px';
                    document.body.appendChild(textArea);
                    textArea.select();
                    
                    try {
                        document.execCommand('copy');
                        showToast('Đã sao chép liên kết!');
                    } catch (err) {
                        console.error('Failed to copy:', err);
                    }
                    
                    document.body.removeChild(textArea);
                }
            });
        }
    });

    // ============================================
    // Toast Notification
    // ============================================
    function showToast(message, duration = 3000) {
        const toast = document.createElement('div');
        toast.className = 'toast-notification';
        toast.innerHTML = `
            <i class="fas fa-check-circle me-2"></i>
            <span>${message}</span>
        `;
        document.body.appendChild(toast);

        setTimeout(() => toast.classList.add('show'), 10);
        
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => document.body.removeChild(toast), 300);
        }, duration);
    }

    // ============================================
    // Enhanced Search Modal
    // ============================================
    const searchModal = document.getElementById('searchModal');
    if (searchModal) {
        searchModal.addEventListener('shown.bs.modal', function () {
            const searchInput = this.querySelector('input[name="keyword"]');
            if (searchInput) {
                searchInput.focus();
            }
        });
    }

    // ============================================
    // Dropdown Menu Accessibility
    // ============================================
    const dropdownToggles = document.querySelectorAll('[data-bs-toggle="dropdown"]');
    dropdownToggles.forEach(toggle => {
        toggle.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                this.click();
            }
        });
    });

    // ============================================
    // Smooth Category Menu Highlight
    // ============================================
    const currentPath = window.location.pathname;
    const categoryLinks = document.querySelectorAll('.nav-link-cat, .category-menu .dropdown-item');
    
    categoryLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
            link.style.color = 'var(--primary-red)';
            
            // If it's a dropdown item, highlight the parent too
            const parentDropdown = link.closest('.dropdown');
            if (parentDropdown) {
                const parentToggle = parentDropdown.querySelector('.dropdown-toggle');
                if (parentToggle) {
                    parentToggle.style.color = 'var(--primary-red)';
                }
            }
        }
    });

    // ============================================
    // Lazy Load Images Enhancement
    // ============================================
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    if (img.dataset.src) {
                        img.src = img.dataset.src;
                        img.classList.add('loaded');
                        observer.unobserve(img);
                    }
                }
            });
        });

        document.querySelectorAll('img[data-src]').forEach(img => {
            imageObserver.observe(img);
        });
    }

    // ============================================
    // Timeline Auto-scroll Preview
    // ============================================
    const timeline = document.querySelector('.latest-timeline');
    if (timeline) {
        let scrollTimeout;
        
        timeline.addEventListener('mouseenter', function () {
            clearTimeout(scrollTimeout);
            this.style.scrollBehavior = 'smooth';
        });

        timeline.addEventListener('mouseleave', function () {
            scrollTimeout = setTimeout(() => {
                this.scrollTop = 0;
            }, 2000);
        });
    }

    // ============================================
    // Print Article Button
    // ============================================
    if (document.querySelector('.article-detail')) {
        const printButton = document.createElement('button');
        printButton.className = 'btn btn-outline-secondary btn-sm ms-2';
        printButton.innerHTML = '<i class="fas fa-print me-1"></i>In bài viết';
        printButton.addEventListener('click', () => window.print());
        
        const shareButtons = document.querySelector('.share-buttons');
        if (shareButtons) {
            shareButtons.appendChild(printButton);
        }
    }

    // ============================================
    // Prevent FOUC (Flash of Unstyled Content)
    // ============================================
    document.documentElement.classList.add('js-loaded');

    // ============================================
    // Mobile Menu Close on Link Click
    // ============================================
    const offcanvasLinks = document.querySelectorAll('.offcanvas-body a:not([data-bs-toggle])');
    const offcanvas = document.querySelector('.offcanvas');
    
    if (offcanvas) {
        const bsOffcanvas = bootstrap.Offcanvas.getInstance(offcanvas) || new bootstrap.Offcanvas(offcanvas);
        
        offcanvasLinks.forEach(link => {
            link.addEventListener('click', function () {
                setTimeout(() => {
                    bsOffcanvas.hide();
                }, 300);
            });
        });
    }

    // ============================================
    // Badge Hover Effect
    // ============================================
    const badges = document.querySelectorAll('.badge');
    badges.forEach(badge => {
        badge.addEventListener('mouseenter', function () {
            this.style.transform = 'scale(1.1)';
        });
        
        badge.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
        });
    });

    console.log('🎨 SacViet Enhanced UX với Smooth Scroll đã tải thành công!');

})();
