using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a bloodbane fox corpse")]
    public class BloodbaneFox : BaseCreature
    {
        private DateTime m_NextHowlTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextDrainTime;
        private Point3D m_LastLocation;

        // A deep, blood-red hue
        private const int UniqueHue = 1175;

        [Constructable]
        public BloodbaneFox() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name = "Bloodbane Fox";
            Body = 0x58f;                  // Same as Blood Fox
            BaseSoundID = 0x8E;           // Fox-like sound
            Hue = UniqueHue;

            // -- Enhanced Stats --
            SetStr(400, 450);
            SetDex(300, 350);
            SetInt(200, 250);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(150, 200);

            SetDamage(25, 35);

            // Damage split: mostly physical, some cold
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.DetectHidden, 80.0, 90.0);

            Fame = 17500;
            Karma = -17500;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextHowlTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRiftTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextDrainTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation  = this.Location;

            // Loot: bone shards, gold, chance at unique cloak
            PackItem(new Bone(Utility.RandomMinMax(20, 30)));
            PackGold(200, 300);
        }

        // -- Leave quicksand patches as it moves --
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || Map == null || Map == Map.Internal)
                return;

            // 20% chance to leave behind a quicksand hazard at its previous square
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var loc = m_LastLocation;
                int z = loc.Z;

                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Essence Drain: up close burst of life & mana drain
            if (now >= m_NextDrainTime && this.InRange(Combatant.Location, 2))
            {
                EssenceDrainAttack();
                m_NextDrainTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Unstable Rift: targeted poison hazard
            else if (now >= m_NextRiftTime && this.InRange(Combatant.Location, 12))
            {
                UnstableRiftAttack();
                m_NextRiftTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            // Howl of Ruin: fearsome AoE
            else if (now >= m_NextHowlTime && this.InRange(Combatant.Location, 8))
            {
                HowlOfRuin();
                m_NextHowlTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // -- Drains hit points and mana from the primary target --
        private void EssenceDrainAttack()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.PlaySound(0x2F9);
                this.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                target.SendMessage(0x22, "The Bloodbane Fox's gaze sears your life and mana!");

                int hpDrain = Utility.RandomMinMax(50, 80);
                int mpDrain = Utility.RandomMinMax(20, 40);

                AOS.Damage(target, this, hpDrain, 0, 0, 0, 0, 100);

                if (target.Mana >= mpDrain)
                    target.Mana -= mpDrain;
            }
        }

        // -- Rifts a toxic poison tile beneath the target after a brief tear in reality --
        private void UnstableRiftAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Reality shatters!*");
            PlaySound(0x22F);
            var loc = target.Location;

            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                int z = loc.Z;
                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                var poison = new PoisonTile();
                poison.Hue = UniqueHue;
                poison.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
                Effects.PlaySound(loc, Map, 0x215);
            });
        }

        // -- Emits a terrifying howl that damages and briefly stuns nearby foes --
        private void HowlOfRuin()
        {
            if (Map == null) return;

            this.Say("*Wrrrrrlllll!*");
            PlaySound(0x1F4);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0
            );

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                DoHarmful(t);
                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(t, this, dmg, 100, 0, 0, 0, 0);
                t.SendMessage("You reel from the blood-chilling howl!");
                t.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)));
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            this.Say("*My blood… remains!*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            // Spawn hazards around the corpse
            int count = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < count; i++)
            {
                int xOff = Utility.RandomMinMax(-4, 4);
                int yOff = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + xOff, Y + yOff, Z);
                int z = loc.Z;

                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                // Alternate between poison and quicksand
                if (i % 2 == 0)
                {
                    var poison = new PoisonTile();
                    poison.Hue = UniqueHue;
                    poison.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
                }
                else
                {
                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));
            if (Utility.RandomDouble() < 0.02) // 2% for unique cloak
                PackItem(new BloodFoxCloak());
        }

        // Standard properties
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus    { get { return 70.0; } }

        public BloodbaneFox(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize timers after load
            var now = DateTime.UtcNow;
            m_NextHowlTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRiftTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextDrainTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation  = this.Location;
        }
    }
}
