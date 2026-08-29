<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreatePurchaseOfLandLoRowsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('purchase_of_land_lo_rows', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('deed_id');
            $table->string('lo_name')->nullable();
            $table->string('so')->nullable();
            $table->string('lo_cnic')->nullable();
            $table->string('contact_no')->nullable();
            $table->timestamps();
            
            // Foreign key
            $table->foreign('deed_id')->references('id')->on('Purchase_of_lands')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('purchase_of_land_lo_rows');
    }
}
