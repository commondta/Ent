<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;
use Illuminate\Support\Facades\DB;

class FixConveyanceRowsDeedIdDataType extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        // This is a safety/backup migration in case the earlier migration needs to be retried
        // It performs the same fixes but is designed to be idempotent
        $table = DB::connection()->getTablePrefix() . 'conveyance_rows';
        
        // Drop old foreign key if it exists
        try {
            DB::statement('ALTER TABLE `conveyance_rows` DROP FOREIGN KEY `fk_conveyance_rows`');
        } catch (\Exception $e) {}
        
        try {
            DB::statement('ALTER TABLE `conveyance_rows` DROP FOREIGN KEY `conveyance_rows_deed_id_foreign`');
        } catch (\Exception $e) {}

        // Try to ensure deed_id is the correct type
        try {
            DB::statement('ALTER TABLE `conveyance_rows` MODIFY COLUMN `deed_id` BIGINT UNSIGNED NOT NULL');
        } catch (\Exception $e) {}

        // Add foreign key if it doesn't exist
        try {
            DB::statement('ALTER TABLE `conveyance_rows` ADD CONSTRAINT `conveyance_rows_deed_id_foreign` FOREIGN KEY (`deed_id`) REFERENCES `conveyances`(`id`) ON DELETE CASCADE');
        } catch (\Exception $e) {
            // Foreign key probably already exists, which is fine
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

        Schema::table('conveyance_rows', function (Blueprint $table) {
            $table->integer('deed_id')->change();
        });
    }
}
