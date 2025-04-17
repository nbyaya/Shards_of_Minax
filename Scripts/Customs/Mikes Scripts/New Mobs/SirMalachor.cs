using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class SirMalachor : BaseCreature
    {
        private int m_Phase;
        private DateTime m_NextFollowerSummon;
        private DateTime m_NextArcherSummon;

        [Constructable]
        public SirMalachor() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "Sir Malachor, the Dread Knight";
            Body = 400; // Use a custom body id or your preferred monster body.
            Hue = 1175; // A deep, ominous hue.
            m_Phase = 1;

            // Set basic stats.
            SetStr(1500, 2000);
            SetDex(200, 300);
            SetInt(1000, 1200);
            SetHits(4000, 5000);
            SetDamage(20, 30);

            // Equip Sir Malachor with his uniquely named armor.
            AddItem(new UniqueHelmet());
            AddItem(new UniqueChestplate());
            AddItem(new UniqueLegplates());
            AddItem(new UniqueGauntlets());
            AddItem(new UniqueBoots());

            // Initialize summon timers.
            m_NextFollowerSummon = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextArcherSummon = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // Check for phase transitions based on remaining hits.
            if (m_Phase == 1 && Hits < HitsMax * 0.75)
            {
                m_Phase = 2;
                Emote("*Sir Malachor roars: 'You dare wound me? Face my wrath!'*");
                // Immediately summon three loyal followers.
                SummonLoyalFollowers(3);
            }
            else if (m_Phase == 2 && Hits < HitsMax * 0.50)
            {
                m_Phase = 3;
                Emote("*Sir Malachor shouts: 'Now, witness true terror!'*");
                // Summon a pair of archers.
                SummonArchers(2);
            }
            else if (m_Phase == 3 && Hits < HitsMax * 0.25)
            {
                m_Phase = 4;
                Emote("*Sir Malachor bellows: 'I will finish you myself!'*");
                // Enraged state: boost his melee power and add more minions.
                this.DamageMin = (int)(this.DamageMin * 1.5);
                this.DamageMax = (int)(this.DamageMax * 1.5);
                SummonLoyalFollowers(2);
                SummonArchers(1);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // During later phases, periodically summon additional minions.
            if (m_Phase >= 2 && DateTime.UtcNow >= m_NextFollowerSummon)
            {
                SummonLoyalFollowers(1);
                m_NextFollowerSummon = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
            if (m_Phase >= 3 && DateTime.UtcNow >= m_NextArcherSummon)
            {
                SummonArchers(1);
                m_NextArcherSummon = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        public void SummonLoyalFollowers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                LoyalSquire squire = new LoyalSquire();
                squire.Team = this.Team;
                squire.MoveToWorld(this.Location, this.Map);
                squire.Combatant = this.Combatant;
                PublicOverheadMessage(Server.Network.MessageType.Regular, 0, false, "A loyal squire appears!");
            }
        }

        public void SummonArchers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Archer archer = new Archer();
                archer.Team = this.Team;
                archer.MoveToWorld(this.Location, this.Map);
                archer.Combatant = this.Combatant;
                PublicOverheadMessage(Server.Network.MessageType.Regular, 0, false, "An archer takes aim!");
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Drop unique loot upon death.
            c.DropItem(new LegendarySword());
            c.DropItem(new MysticShield());
            c.DropItem(new KnightlyTabard());
        }

        public SirMalachor(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Phase);
            writer.Write(m_NextFollowerSummon);
            writer.Write(m_NextArcherSummon);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Phase = reader.ReadInt();
            m_NextFollowerSummon = reader.ReadDateTime();
            m_NextArcherSummon = reader.ReadDateTime();
        }
    }

    #region Unique Armor Pieces

    public class UniqueHelmet : PlateHelm
    {
        [Constructable]
        public UniqueHelmet()
        {
            Name = "Helm of Twilight's Echo";
            Hue = 0x455; // A unique blue-gray hue.
            LootType = LootType.Newbied;
        }

        public UniqueHelmet(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class UniqueChestplate : PlateChest
    {
        [Constructable]
        public UniqueChestplate()
        {
            Name = "Breastplate of the Crimson Dawn";
            Hue = 0x21; // A deep red hue.
            LootType = LootType.Newbied;
        }

        public UniqueChestplate(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class UniqueLegplates : PlateArms  // You may substitute with an appropriate legs item.
    {
        [Constructable]
        public UniqueLegplates()
        {
            Name = "Legplates of the Shadowed Vale";
            Hue = 0x47E; // A dark, mysterious hue.
            LootType = LootType.Newbied;
        }

        public UniqueLegplates(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class UniqueGauntlets : PlateGloves
    {
        [Constructable]
        public UniqueGauntlets()
        {
            Name = "Gauntlets of the Forsaken Oath";
            Hue = 0x59; // A striking, unique hue.
            LootType = LootType.Newbied;
        }

        public UniqueGauntlets(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class UniqueBoots : Boots
    {
        [Constructable]
        public UniqueBoots() : base(0x1F4)  // Use an appropriate item id.
        {
            Name = "Boots of the Nightfall March";
            Hue = 0x489; // A unique midnight hue.
            LootType = LootType.Newbied;
        }

        public UniqueBoots(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    #endregion

    #region Unique Loot Items

    public class LegendarySword : Longsword
    {
        [Constructable]
        public LegendarySword() : base()
        {
            Name = "Sword of Eternal Dusk";
            Hue = 0x497;
            Attributes.WeaponDamage = 25;
            Attributes.WeaponSpeed = 15;
        }

        public LegendarySword(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class MysticShield : Shield
    {
        [Constructable]
        public MysticShield() : base()
        {
            Name = "Aegis of the Mystic Eclipse";
            Hue = 0x5A; 
            Attributes.DefendChance = 20;
        }

        public MysticShield(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class KnightlyTabard : Cloak
    {
        [Constructable]
        public KnightlyTabard() : base(0x1C03) // Use an appropriate item id.
        {
            Name = "Tabard of the Dread Knight";
            Hue = 0x455;
        }

        public KnightlyTabard(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    #endregion

    #region Minion Definitions

    // A simple loyal squire minion.
    public class LoyalSquire : BaseCreature
    {
        [Constructable]
        public LoyalSquire() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Loyal Squire";
            Body = 400;
            Hue = 0x83E; // A distinctive hue.
            SetStr(300, 400);
            SetDex(200, 300);
            SetInt(100, 150);
            SetHits(500, 600);
            SetDamage(10, 15);
        }

        public LoyalSquire(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    // A ranged archer minion.
    public class Archer : BaseCreature
    {
        [Constructable]
        public Archer() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Archer";
            Body = 400;
            Hue = 0x648;
            SetStr(300, 400);
            SetDex(400, 500);
            SetInt(100, 150);
            SetHits(500, 600);
            SetDamage(15, 20);

            // Configure ranged weapon ability (implementation depends on your server)
            // For example, you might equip a bow and set a ranged combat ability.
        }

        public Archer(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    #endregion
}
