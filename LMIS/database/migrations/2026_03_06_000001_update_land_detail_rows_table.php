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
        Schema::table('land_detail_rows', function (Blueprint $table) {
            // Remove old columns
            if (Schema::hasColumn('land_detail_rows', 'rectangle_no')) {
                $table->dropColumn('rectangle_no');
            }
            if (Schema::hasColumn('land_detail_rows', 'muraba_no')) {
                $table->dropColumn('muraba_no');
            }
            if (Schema::hasColumn('land_detail_rows', 'khasra_no')) {
                $table->dropColumn('khasra_no');
            }
            if (Schema::hasColumn('land_detail_rows', 'kanal')) {
                $table->dropColumn('kanal');
            }
            if (Schema::hasColumn('land_detail_rows', 'marla')) {
                $table->dropColumn('marla');
            }
            if (Schema::hasColumn('land_detail_rows', 'sq_feet')) {
                $table->dropColumn('sq_feet');
            }

            // Add new columns after khatooni_no (check if they don't already exist)
            if (!Schema::hasColumn('land_detail_rows', 'qatat')) {
                $table->string('qatat')->nullable()->after('khatooni_no');
            }
            
            // Measuring columns
            if (!Schema::hasColumn('land_detail_rows', 'measuring_k')) {
                $table->string('measuring_k')->nullable()->after('qatat');
            }
            if (!Schema::hasColumn('land_detail_rows', 'measuring_m')) {
                $table->string('measuring_m')->nullable()->after('measuring_k');
            }
            if (!Schema::hasColumn('land_detail_rows', 'measuring_sqft')) {
                $table->string('measuring_sqft')->nullable()->after('measuring_m');
            }
            
            // Transfer Share column
            if (!Schema::hasColumn('land_detail_rows', 'transfer_share')) {
                $table->string('transfer_share')->nullable()->after('measuring_sqft');
            }
            
            // Land Measuring columns
            if (!Schema::hasColumn('land_detail_rows', 'land_measuring_k')) {
                $table->string('land_measuring_k')->nullable()->after('transfer_share');
            }
            if (!Schema::hasColumn('land_detail_rows', 'land_measuring_m')) {
                $table->string('land_measuring_m')->nullable()->after('land_measuring_k');
            }
            if (!Schema::hasColumn('land_detail_rows', 'land_measuring_sqft')) {
                $table->string('land_measuring_sqft')->nullable()->after('land_measuring_m');
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
        Schema::table('land_detail_rows', function (Blueprint $table) {
            // Revert by adding back old columns
            $table->string('rectangle_no')->nullable()->after('khatooni_no');
            $table->string('muraba_no')->nullable()->after('rectangle_no');
            $table->string('khasra_no')->nullable()->after('muraba_no');
            $table->string('kanal')->nullable()->after('khasra_no');
            $table->string('marla')->nullable()->after('kanal');
            $table->string('sq_feet')->nullable()->after('marla');

            // Drop new columns
            $table->dropColumn([
                'qatat',
                'measuring_k',
                'measuring_m',
                'measuring_sqft',
                'transfer_share',
                'land_measuring_k',
                'land_measuring_m',
                'land_measuring_sqft'
            ]);
        });
    }
};
