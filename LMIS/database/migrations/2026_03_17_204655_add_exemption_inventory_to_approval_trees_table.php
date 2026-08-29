<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddExemptionInventoryToApprovalTreesTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('approval_trees', function (Blueprint $table) {
            $table->integer('exemption_inventory')->default(0)->after('intimation_letter');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::table('approval_trees', function (Blueprint $table) {
            $table->dropColumn('exemption_inventory');
        });
    }
}
