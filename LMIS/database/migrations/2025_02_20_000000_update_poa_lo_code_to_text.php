<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class UpdatePoaLoCodeToText extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('Land_forms', function (Blueprint $table) {
            // Change poa_lo_code column to text to support multiple comma-separated values
            if (Schema::hasColumn('Land_forms', 'poa_lo_code')) {
                $table->text('poa_lo_code')->nullable()->change();
            } else {
                $table->text('poa_lo_code')->nullable();
            }
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::table('Land_forms', function (Blueprint $table) {
            // Revert to string type
            if (Schema::hasColumn('Land_forms', 'poa_lo_code')) {
                $table->string('poa_lo_code')->nullable()->change();
            }
        });
    }
}
