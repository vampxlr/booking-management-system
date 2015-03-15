namespace SimchaMVC
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
using SimchaMVC.Models;

    public partial class SimchaDB : DbContext
    {
        public SimchaDB()
            : base("name=SimchaDBLocal")
        {
        }
        public virtual DbSet<special_time_slots> special_time_slots { get; set; }
        public  DbSet<admin_users> admin_users { get; set; }
        public virtual DbSet<booking_logs> booking_logs { get; set; }
        public virtual DbSet<booking> bookings { get; set; }
        public virtual DbSet<calendar> calendars { get; set; }
        public virtual DbSet<caterer> caterers { get; set; }
        public virtual DbSet<caterers_hashgachot> caterers_hashgachot { get; set; }
        public virtual DbSet<event_types> event_types { get; set; }
        public virtual DbSet<hall_areas> hall_areas { get; set; }
        public virtual DbSet<hall_caterers> hall_caterers { get; set; }
        public virtual DbSet<hall_event_types> hall_event_types { get; set; }
        public virtual DbSet<hall_images> hall_images { get; set; }
        public virtual DbSet<hall> halls { get; set; }
        public virtual DbSet<hashgacha_images> hashgacha_images { get; set; }
        public virtual DbSet<service_areas> service_areas { get; set; }
        public virtual DbSet<slot_numbers> slot_numbers { get; set; }
        public virtual DbSet<time_slots> time_slots { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<weekday> weekdays { get; set; }
        public virtual DbSet<wishlist> wishlists { get; set; }
        public virtual DbSet<zip> zips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.user_name)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.user_password)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.first_name)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.last_name)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.address)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.city)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.state)
            //    .IsUnicode(false);

            //modelBuilder.Entity<admin_users>()
            //    .Property(e => e.zipcode)
            //    .IsUnicode(false);

            modelBuilder.Entity<booking_logs>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.time_slot)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.booking_status)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.customer_notes)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.card_type)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.card_expiration)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.card_number)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.payment_type)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.cvv)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.simcha_type)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.internal_notes)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.internal_status)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.address1)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.address2)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.state)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.zip_code)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.fax)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.name_on_card)
                .IsUnicode(false);

            modelBuilder.Entity<calendar>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<calendar>()
                .Property(e => e.special_price)
                .IsUnicode(false);

            modelBuilder.Entity<caterer>()
                .Property(e => e.caterer_name)
                .IsUnicode(false);

            modelBuilder.Entity<caterer>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<caterer>()
                .Property(e => e.price)
                .IsUnicode(false);

            modelBuilder.Entity<event_types>()
                .Property(e => e.type_name)
                .IsUnicode(false);

            modelBuilder.Entity<hall_images>()
                .Property(e => e.image_name)
                .IsUnicode(false);

            modelBuilder.Entity<hall_images>()
                .Property(e => e.image_caption)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.hall_name)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.contact_name)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.address1)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.address2)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.state)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.zip_code)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.phone2)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.fax)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.website)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.capacity)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.policies)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.features)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.direction_doc)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.directions)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.preferred_contact_method)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.service_area)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.hall_pricing)
                .IsUnicode(false);

            modelBuilder.Entity<hall>()
                .Property(e => e.office_hours)
                .IsUnicode(false);

            modelBuilder.Entity<hashgacha_images>()
                .Property(e => e.hashgacha_name)
                .IsUnicode(false);

            modelBuilder.Entity<hashgacha_images>()
                .Property(e => e.hashgacha_image)
                .IsUnicode(false);

            modelBuilder.Entity<hashgacha_images>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<service_areas>()
                .Property(e => e.service_area)
                .IsUnicode(false);

            modelBuilder.Entity<time_slots>()
                .Property(e => e.time_slot)
                .IsUnicode(false);

            modelBuilder.Entity<time_slots>()
                .Property(e => e.slot_amount)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.user_password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.address1)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.address2)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.state)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.zipcode)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.fax)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.internal_notes)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password_hint)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.cell_phone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.best_time)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.contact_method)
                .IsUnicode(false);

            modelBuilder.Entity<weekday>()
                .Property(e => e.day_name)
                .IsUnicode(false);

            modelBuilder.Entity<wishlist>()
                .Property(e => e.event_date)
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.zip1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.state)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.lat)
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.lon)
                .IsUnicode(false);

            modelBuilder.Entity<zip>()
                .Property(e => e.dst)
                .IsUnicode(false);
        }
    }
}
