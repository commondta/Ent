<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateExemptionInventoryApprovalsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('exemption_inventory_approvals', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no')->unique();
            $table->date('date')->nullable();
            $table->string('land_offer_form_no')->nullable();
            $table->decimal('total_registered_land', 10, 2)->nullable();
            $table->decimal('total_possessed_land', 10, 2)->nullable();
            $table->decimal('rate_per_acre', 10, 2)->nullable();
            $table->decimal('total_cost_registered', 10, 2)->nullable();
            $table->decimal('total_cost_possessed', 10, 2)->nullable();
            $table->decimal('total_residential_files', 10, 2)->nullable();
            $table->decimal('total_commercial_files', 10, 2)->nullable();
            $table->decimal('exemption_percent', 10, 2)->nullable();
            $table->decimal('total_cost', 10, 2)->nullable();
            $table->decimal('residential_percent', 10, 2)->nullable();
            $table->decimal('commercial_percent', 10, 2)->nullable();
            $table->decimal('cash_decimal', 10, 2)->nullable();
            $table->text('remarks')->nullable();
            $table->integer('status')->default(0);
            $table->boolean('isDeleted')->default(0);
            $table->integer('createdBy')->nullable();
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
        Schema::dropIfExists('exemption_inventory_approvals');
    }
}
