<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddAttachmentColumnToExemptionInventory extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('exemption_inventory_approvals', function (Blueprint $table) {
            if (!Schema::hasColumn('exemption_inventory_approvals', 'attachment')) {
                $table->string('attachment')->nullable()->after('remarks');
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
        Schema::table('exemption_inventory_approvals', function (Blueprint $table) {
            if (Schema::hasColumn('exemption_inventory_approvals', 'attachment')) {
                $table->dropColumn('attachment');
            }
        });
    }
}
