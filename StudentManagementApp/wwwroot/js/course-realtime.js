window.meshCourseRealtime = window.meshCourseRealtime || {
    fallbackImageUrl: "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?q=80&w=2022&auto=format&fit=crop",

    connect(options) {
        if (typeof signalR === "undefined") {
            return Promise.resolve(null);
        }

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/notifications")
            .withAutomaticReconnect()
            .build();

        if (options && typeof options.onCourseChanged === "function") {
            connection.on("CourseChanged", options.onCourseChanged);
        }

        return connection.start()
            .then(() => connection)
            .catch((error) => {
                console.error("SignalR course realtime connection failed.", error);
                return null;
            });
    },

    escapeHtml(value) {
        return String(value ?? "")
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#39;");
    },

    getCourseImageUrl(course) {
        return course && course.thumbnailUrl
            ? course.thumbnailUrl
            : this.fallbackImageUrl;
    },

    formatCurrency(value) {
        const amount = Number(value ?? 0);
        return `${new Intl.NumberFormat("vi-VN").format(amount)} VNĐ`;
    },

    buildCourseCardMarkup(course) {
        const name = this.escapeHtml(course.name);
        const level = this.escapeHtml(course.level);
        const description = this.escapeHtml(course.description || "Khóa học đang được cập nhật nội dung.");
        const imageUrl = this.escapeHtml(this.getCourseImageUrl(course));
        const price = this.escapeHtml(this.formatCurrency(course.tuitionFee));

        return `
            <div class="res-card d-flex flex-column h-100">
                <div class="position-relative">
                    <img src="${imageUrl}" class="card-img-top" alt="${name}" style="height: 200px; object-fit: cover;">
                    <div class="position-absolute top-0 end-0 m-3">
                        <span class="badge bg-res-red text-white px-3 py-2 rounded-pill fw-bold">${level}</span>
                    </div>
                </div>
                <div class="card-body p-4 flex-grow-1">
                    <h5 class="fw-bold mb-3" data-role="course-name">${name}</h5>
                    <p class="text-muted small mb-4 text-truncate-3" data-role="course-description">${description}</p>
                    <div class="d-flex justify-content-between align-items-center mt-auto">
                        <div class="text-res-red fw-bold h5 mb-0" data-role="course-fee">${price}</div>
                        <a href="/Course/Detail/${course.id}" class="btn btn-outline-res btn-sm">CHI TIẾT</a>
                    </div>
                </div>
            </div>`;
    },

    createCourseCard(course) {
        const column = document.createElement("div");
        column.className = "col-lg-4 col-md-6";
        column.dataset.courseId = String(course.id);
        column.innerHTML = this.buildCourseCardMarkup(course);
        return column;
    },

    upsertCourseCard(grid, course) {
        if (!grid || !course) {
            return false;
        }

        if (!course.isActive) {
            return this.removeCourseCard(grid, course.id);
        }

        const newCard = this.createCourseCard(course);
        const selector = `[data-course-id="${course.id}"]`;
        const existingCard = grid.querySelector(selector);
        if (existingCard) {
            existingCard.replaceWith(newCard);
            return true;
        }

        const cards = Array.from(grid.querySelectorAll("[data-course-id]"));
        const nextCard = cards.find((card) => Number(card.dataset.courseId) > Number(course.id));
        if (nextCard) {
            grid.insertBefore(newCard, nextCard);
            return true;
        }

        grid.appendChild(newCard);
        return true;
    },

    removeCourseCard(grid, courseId) {
        if (!grid) {
            return false;
        }

        const existingCard = grid.querySelector(`[data-course-id="${courseId}"]`);
        if (!existingCard) {
            return false;
        }

        existingCard.remove();
        return true;
    },

    syncEmptyState(grid, emptyState) {
        if (!emptyState || !grid) {
            return;
        }

        const hasCards = !!grid.querySelector("[data-course-id]");
        emptyState.hidden = hasCards;
    }
};
