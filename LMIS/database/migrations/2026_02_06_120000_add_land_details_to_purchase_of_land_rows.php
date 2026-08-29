<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddLandDetailsToPurchaseOfLandRows extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('purchase_of_land_rows', function (Blueprint $table) {
            $table->string('khewat_no')->nullable();
            $table->string('khatooni_no')->nullable();
            $table->string('block_no')->nullable();
            $table->string('rectangle_no')->nullable();
            $table->string('khasra_no')->nullable();
            $table->string('qatat')->nullable();
            $table->string('measuring_k')->nullable();
            $table->string('measuring_m')->nullable();
            $table->string('measuring_sqft')->nullable();
            $table->string('transfer_share')->nullable();
            $table->string('land_measuring_k')->nullable();
            $table->string('land_measuring_m')->nullable();
            $table->string('land_measuring_sqft')->nullable();
            $table->string('land_category')->nullable();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::table('purchase_of_land_rows', function (Blueprint $table) {
            $table->dropColumn([
                'khewat_no',
                'khatooni_no',
                'block_no',
                'rectangle_no',
                'khasra_no',
                'qatat',
                'measuring_k',
                'measuring_m',
                'measuring_sqft',
                'transfer_share',
                'land_measuring_k',
                'land_measuring_m',
                'land_measuring_sqft',
                'land_category',
            ]);
        });
    }
}
