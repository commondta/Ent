<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class ConveyanceRow extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('conveyance_rows', function (Blueprint $table) {
            $table->id();
            $table->integer('deed_id');
            $table->string('block_no');
            $table->integer('rectangle_no');
            $table->string('khasra_no');
            $table->string('east_by');
            $table->string('west_by');
            $table->string('north_by');
            $table->string('south_by');
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
