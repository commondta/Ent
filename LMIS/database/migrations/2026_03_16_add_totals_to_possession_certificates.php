<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('possession_certificates', function (Blueprint $table) {
            // Add total columns if they don't already exist
            if (!Schema::hasColumn('possession_certificates', 'total_land_kanal')) {
                $table->string('total_land_kanal')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_land_marla')) {
                $table->string('total_land_marla')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_land_sqft')) {
                $table->string('total_land_sqft')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_poss_kanal')) {
                $table->string('total_poss_kanal')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_poss_marla')) {
                $table->string('total_poss_marla')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_poss_sqft')) {
                $table->string('total_poss_sqft')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_unposs_kanal')) {
                $table->string('total_unposs_kanal')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_unposs_marla')) {
                $table->string('total_unposs_marla')->nullable();
            }
            if (!Schema::hasColumn('possession_certificates', 'total_unposs_sqft')) {
                $table->string('total_unposs_sqft')->nullable();
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
        Schema::table('possession_certificates', function (Blueprint $table) {
            if (Schema::hasColumn('possession_certificates', 'total_land_kanal')) {
                $table->dropColumn('total_land_kanal');
            }
            if (Schema::hasColumn('possession_certificates', 'total_land_marla')) {
                $table->dropColumn('total_land_marla');
            }
            if (Schema::hasColumn('possession_certificates', 'total_land_sqft')) {
                $table->dropColumn('total_land_sqft');
            }
            if (Schema::hasColumn('possession_certificates', 'total_poss_kanal')) {
                $table->dropColumn('total_poss_kanal');
            }
            if (Schema::hasColumn('possession_certificates', 'total_poss_marla')) {
                $table->dropColumn('total_poss_marla');
            }
            if (Schema::hasColumn('possession_certificates', 'total_poss_sqft')) {
                $table->dropColumn('total_poss_sqft');
            }
            if (Schema::hasColumn('possession_certificates', 'total_unposs_kanal')) {
                $table->dropColumn('total_unposs_kanal');
            }
            if (Schema::hasColumn('possession_certificates', 'total_unposs_marla')) {
                $table->dropColumn('total_unposs_marla');
            }
            if (Schema::hasColumn('possession_certificates', 'total_unposs_sqft')) {
                $table->dropColumn('total_unposs_sqft');
            }
        });
    }
};
