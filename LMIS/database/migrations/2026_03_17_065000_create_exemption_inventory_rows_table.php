<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateExemptionInventoryRowsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('exemption_inventory_rows', function (Blueprint $table) {
            $table->id();
            $table->integer('exemption_inventory_id');
            $table->string('category')->nullable(); // Residential/Commercial
            $table->string('inventory_type')->nullable(); // Files/Plots
            $table->integer('size_of_file')->nullable();
            $table->integer('no_of_files')->nullable();
            $table->decimal('rate_file_plot', 10, 2)->nullable();
            $table->decimal('total_cost', 10, 2)->nullable();
            $table->integer('eighty_percent')->nullable();
            $table->integer('twenty_percent')->nullable();
            $table->text('remark')->nullable();
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
        Schema::dropIfExists('exemption_inventory_rows');
    }
}
