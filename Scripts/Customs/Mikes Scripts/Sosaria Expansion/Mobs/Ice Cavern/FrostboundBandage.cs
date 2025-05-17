using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a frostbound bandage corpse")]
    public class FrostboundBandage : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextBind;
        private DateTime m_NextReform;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostboundBandage()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frostbound bandage";
            Body = 154; // Same body as PestilentBandage
            Hue = 0x47E; // Icy blue hue
            BaseSoundID = 471;

            SetStr(820, 880);
            SetDex(160, 200);
            SetInt(250, 300);

            SetHits(950, 1100);
            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 70);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 90.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);

            Fame = 26000;
            Karma = -26000;
            VirtualArmor = 60;

            PackItem(new Bandage(10)); // Thematic

            m_AbilitiesInitialized = false;
        }

        public override Poison HitPoison => Poison.Greater;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 4);

            if (Utility.RandomDouble() < 0.005) // 0.5% chance
                PackItem(new IceboundPhylactery()); // Unique drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_AbilitiesInitialized)
            {
                m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
                m_NextBind = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                m_NextReform = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(45, 90));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextFrostNova)
                FrostNova();

            if (DateTime.UtcNow >= m_NextBind)
                IceBind();

            if (DateTime.UtcNow >= m_NextReform && Hits < (HitsMax / 2))
                FrozenReform();
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* Frost explodes outward in a violent wave! *");
            PlaySound(0x10B); // Cold sound
            Effects.SendLocationEffect(Location, Map, 0x376A, 20, 10);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);

                    if (m is Mobile mob)
                    {
                        mob.SendMessage("You are blasted by the chilling burst!");
                        mob.Freeze(TimeSpan.FromSeconds(2));
                    }
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 45));
        }

        private void IceBind()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The bandages stretch and freeze around its target! *");
                PlaySound(0x208); // Entangle sound
                target.SendMessage("You are bound in icy wrappings!");
                target.Freeze(TimeSpan.FromSeconds(3));

                if (Utility.RandomDouble() < 0.4)
                {
                    target.AddStatMod(new StatMod(StatType.Dex, "FrostDex", -15, TimeSpan.FromSeconds(10)));
                    target.SendMessage("The frost saps your agility!");
                }
            }

            m_NextBind = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 25));
        }

        private void FrozenReform()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47E, true, "* The frostbound bandage reforms its tattered body! *");
            PlaySound(0x5C6); // Rebirth sound
            FixedParticles(0x376A, 10, 25, 5032, Hue, 0, EffectLayer.Waist);

            Heal(Utility.RandomMinMax(80, 150));
            Stam = StamMax;

            m_NextReform = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(60, 90));
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is Mobile target && Utility.RandomDouble() < 0.25)
            {
                target.SendMessage("Chilling pain radiates from the wound!");
                AOS.Damage(target, this, Utility.RandomMinMax(8, 16), 0, 100, 0, 0, 0);
                target.Freeze(TimeSpan.FromSeconds(1));
            }
        }

        public FrostboundBandage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class IceboundPhylactery : Item
    {
        [Constructable]
        public IceboundPhylactery() : base(0x2B97) // Unique graphic
        {
            Name = "Icebound Phylactery";
            Hue = 0x47E;
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public IceboundPhylactery(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
