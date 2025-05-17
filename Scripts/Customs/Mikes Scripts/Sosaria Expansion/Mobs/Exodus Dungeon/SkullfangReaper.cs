using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a skullfang reaper corpse")]
    public class SkullfangReaper : BaseCreature
    {
        private DateTime m_NextSoulHarvest;
        private DateTime m_NextShadowLash;
        private DateTime m_NextFearPulse;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SkullfangReaper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Skullfang Reaper";
            Body = 0x190; // Same body as the base mob
            BaseSoundID = 0x1FB; // Reuse serpent fang sounds
            Hue = 0x497; // Unique spectral blue-green hue

            SetStr(900, 1000);
            SetDex(200, 250);
            SetInt(400, 500);

            SetHits(1800, 2200);
            SetStam(250, 350);
            SetMana(1000, 1200);

            SetDamage(28, 36);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 130.0);

            Fame = 28000;
            Karma = -28000;

            VirtualArmor = 85;

            m_AbilitiesInitialized = false;
        }

        public SkullfangReaper(Serial serial) : base(serial) { }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new ShadowFangCharm()); // Rare drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && !m_AbilitiesInitialized)
            {
                m_NextSoulHarvest = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                m_NextShadowLash = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                m_NextFearPulse = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextSoulHarvest)
                SoulHarvest();

            if (DateTime.UtcNow >= m_NextShadowLash)
                ShadowLash();

            if (DateTime.UtcNow >= m_NextFearPulse)
                FearPulse();
        }

        private void SoulHarvest()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x22, true, "* The Reaper harvests the essence of your soul! *");
                PlaySound(0x1F1); // Spectral wail
                AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 0, 100, 0);
                target.Mana -= 30;

                if (target.Mana < 0) target.Mana = 0;
                if (target.Stam > 10) target.Stam -= 10;

                Hits += 20; // Minor self-heal
            }

            m_NextSoulHarvest = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void ShadowLash()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x22, true, "* A lash of pure shadow strikes from the Reaper's blade! *");
                Effects.SendBoltEffect(target, true);
                AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);

                if (Utility.RandomDouble() < 0.2)
                {
                    target.Freeze(TimeSpan.FromSeconds(3));
                    target.SendMessage("You are frozen in shadow!");
                }
            }

            m_NextShadowLash = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void FearPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "* The Reaper emits a pulse of pure dread! *");
            PlaySound(0x5D3);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != null && m != this && m.Alive && !m.IsDeadBondedPet && m is PlayerMobile)
                {
                    m.FixedParticles(0x374A, 10, 25, 5032, EffectLayer.Head);
                    m.SendMessage(0x22, "A wave of fear washes over you!");

                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        m.SendMessage("You are paralyzed by fear!");
                    }

                    m.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }

            m_NextFearPulse = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            // Reflect a portion of melee damage as energy damage
            if (from != null)
                AOS.Damage(from, this, damage / 4, 0, 0, 0, 100, 0);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.2)
                c.DropItem(new SoulshardFragment());

            if (Utility.RandomDouble() < 0.05)
                c.DropItem(new SkullfangBlade());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    // Optional custom drop item
    public class SkullfangBlade : Katana
    {
        [Constructable]
        public SkullfangBlade()
        {
            Name = "Skullfang Blade";
            Hue = 0x497;
            WeaponAttributes.HitLeechMana = 30;
            WeaponAttributes.HitHarm = 40;
            Attributes.WeaponDamage = 50;
            Attributes.SpellChanneling = 1;
        }

        public SkullfangBlade(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ShadowFangCharm : Item
    {
        [Constructable]
        public ShadowFangCharm() : base(0x2F5E)
        {
            Name = "Shadowfang Charm";
            Hue = 0x497;
            LootType = LootType.Blessed;
            Weight = 1.0;
        }

        public ShadowFangCharm(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SoulshardFragment : Item
    {
        [Constructable]
        public SoulshardFragment() : base(0x1EA7)
        {
            Name = "Soulshard Fragment";
            Hue = 0x47E;
            Weight = 0.5;
        }

        public SoulshardFragment(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
