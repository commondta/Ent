<div class="min-h-screen flex flex-col sm:justify-center items-center pt-6 sm:pt-0 bg-gray-100">
    <div>
        <a
                class="d-flex flex-center text-decoration-none mb-4" href="#">
            <div class="d-flex align-items-center fw-bolder fs-5 d-inline-block">
                <img class="brand-logo" src="{{ asset('public/assets/img/lmis-logo.svg') }}" alt="{{ config('app.name', 'LMIS') }}" width="170" /></div>
        </a>



    </div>

    <div class="w-full sm:max-w-md mt-6 px-6 py-4 bg-white shadow-md overflow-hidden sm:rounded-lg">
        <div class="text-center mb-7">
            <h3 class="text-1000">Sign In</h3>

            <p class="text-700">Get access to your account</p>

        </div>
        {{ $slot }}
    </div>
</div>
