<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class ConveyanceLandRows extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('conveyance_land_rows', function (Blueprint $table) {
            $table->id();
            $table->integer('deed_id');
            $table->string('transferred_share');
            $table->string('khewat_no');
            $table->string('khatooni_no');
            $table->string('qatat');
            $table->boolean('isDeleted')->default(0);
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        //
    }
}
