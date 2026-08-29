<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;
use Illuminate\Support\Facades\DB;

class FixConveyanceRowsForeignKey extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        // Drop the incorrect foreign key constraint if it exists
        try {
            DB::statement('ALTER TABLE `conveyance_rows` DROP FOREIGN KEY `fk_conveyance_rows`');
        } catch (\Exception $e) {
            // Foreign key doesn't exist, continue
        }

        try {
            DB::statement('ALTER TABLE `conveyance_rows` DROP FOREIGN KEY `conveyance_rows_deed_id_foreign`');
        } catch (\Exception $e) {
            // Foreign key doesn't exist, continue
        }

        // First, modify deed_id column to BIGINT UNSIGNED to match conveyances.id type
        try {
            DB::statement('ALTER TABLE `conveyance_rows` MODIFY COLUMN `deed_id` BIGINT UNSIGNED NOT NULL');
        } catch (\Exception $e) {
            // Column might already be the correct type, continue
        }

        // Then add the correct foreign key constraint
        try {
            Schema::table('conveyance_rows', function (Blueprint $table) {
                $table->foreign('deed_id')
                    ->references('id')
                    ->on('conveyances')
                    ->onDelete('cascade');
            });
        } catch (\Exception $e) {
            // If it fails, try with raw SQL
            DB::statement('ALTER TABLE `conveyance_rows` ADD CONSTRAINT `conveyance_rows_deed_id_foreign` FOREIGN KEY (`deed_id`) REFERENCES `conveyances`(`id`) ON DELETE CASCADE');
        }
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::table('conveyance_rows', function (Blueprint $table) {
            $table->dropForeign(['deed_id']);
        });
    }
}
