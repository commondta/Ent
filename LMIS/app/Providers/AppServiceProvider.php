<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;

class AppServiceProvider extends ServiceProvider
{
    /**
     * Register any application services.
     *
     * @return void
     */
    public function register()
    {
        //
    }

    /**
     * Bootstrap any application services.
     *
     * @return void
     */
    public function boot()
    {
        // Behind the ERP reverse proxy the public URL is APP_URL (e.g. http://host/lims);
        // generate every link/redirect against it rather than the internal dev-server host.
        if (config('erp.enabled') && config('app.url')) {
            \Illuminate\Support\Facades\URL::forceRootUrl(config('app.url'));
            if (str_starts_with(config('app.url'), 'https://')) {
                \Illuminate\Support\Facades\URL::forceScheme('https');
            }
        }
    }
}
