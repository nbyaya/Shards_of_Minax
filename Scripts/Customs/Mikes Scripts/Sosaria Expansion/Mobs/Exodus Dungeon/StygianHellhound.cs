using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a stygian hellhound corpse")]
    public class StygianHellhound : BaseCreature
    {
        private DateTime m_NextShadowFlame;
        private DateTime m_NextSoulHowl;
        private DateTime m_NextInfernalLeash;

        [Constructable]
        public StygianHellhound()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Stygian Hellhound";
            Body = 98; // same as Hell Hound
            BaseSoundID = 229; // same as Hell Hound
            Hue = 1175; // Fiery black/red hue – customize as desired

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(800, 1200);
            SetMana(500, 700);
            SetStam(250, 350);

            SetDamage(20, 35);
            SetDamageType(ResistanceType.Fire, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 130.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);
            SetSkill(SkillName.Necromancy, 80.0, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            m_NextShadowFlame = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextSoulHowl = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextInfernalLeash = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public StygianHellhound(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.01) // 1% chance
                PackItem(new InfernalCloakOfHounds()); // rare item drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextShadowFlame)
                    ShadowFlameRoar();

                if (DateTime.UtcNow >= m_NextSoulHowl)
                    SoulHowl();

                if (DateTime.UtcNow >= m_NextInfernalLeash)
                    InfernalLeash();
            }
        }

        private void ShadowFlameRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x660, true, "*The Stygian Hellhound roars, unleashing shadowflame!*");
            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 50, 50, 0, 0, 0);
                    m.SendMessage("You are scorched by black fire!");
                    m.ApplyPoison(this, Poison.Regular);
                }
            }

            m_NextShadowFlame = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SoulHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x660, true, "*The Stygian Hellhound lets out a soul-piercing howl!*");
            PlaySound(0x20C);

            if (Combatant is Mobile target)
            {
                target.Mana -= 30;
                target.Stam -= 30;
                target.SendMessage("Your soul shudders, draining your energy!");

                // Fear effect (temporary flee)
                target.Freeze(TimeSpan.FromSeconds(2));
            }

            m_NextSoulHowl = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void InfernalLeash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x660, true, "*Chains of fire lash out!*");

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    Effects.SendBoltEffect(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);

                    if (m is Mobile mob)
                    {
                        mob.SendMessage("You are yanked toward the beast by burning chains!");
                        mob.Location = new Point3D(Location.X, Location.Y, Location.Z); // teleport to hound
                    }
                }
            }

            m_NextInfernalLeash = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Canine;

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
