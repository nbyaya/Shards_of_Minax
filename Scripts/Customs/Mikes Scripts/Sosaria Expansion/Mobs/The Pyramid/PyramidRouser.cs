using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Second;      // For MirrorImageSpell
using Server.Spells.Necromancy;  // For BoneMummy


namespace Server.Mobiles
{
    [CorpseName("a pyramid rouser corpse")]
    public class PyramidRouser : BaseCreature
    {
        private DateTime m_NextSandstorm;
        private DateTime m_NextSummon;
        private DateTime m_NextCurse;
        private DateTime m_NextMirror;
        private Point3D m_LastLocation;

        // A desert-gold hue for this boss
        private const int UniqueHue = 2635;

        [Constructable]
        public PyramidRouser() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name        = "a Pyramid Rouser";
            Title       = "the Desert's Wrath";
            Race        = Race.Gargoyle;
            Hue         = UniqueHue;

            // Mirror the GargishRouser body
            if (Utility.RandomBool())
            {
                Body   = 666;
                Female = false;
                Name   = NameList.RandomName("Gargoyle Male");
            }
            else
            {
                Body   = 667;
                Female = true;
                Name   = NameList.RandomName("Gargoyle Female");
            }

            Utility.AssignRandomHair(this, true);
            if (!Female)
                Utility.AssignRandomFacialHair(this, true);

            // -- Enhanced Attributes --
            SetStr(400, 500);
            SetDex(300, 350);
            SetInt(550, 650);

            SetHits(1800, 2200);
            SetStam(300, 350);
            SetMana(800, 1000);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   40, 50);
            SetResistance(ResistanceType.Energy,   90, 100);

            SetSkill(SkillName.Magery,       115.0, 130.0);
            SetSkill(SkillName.EvalInt,      110.0, 125.0);
            SetSkill(SkillName.MagicResist,  120.0, 135.0);
            SetSkill(SkillName.Meditation,   105.0, 115.0);
            SetSkill(SkillName.Tactics,       95.0, 105.0);
            SetSkill(SkillName.Wrestling,     95.0, 105.0);

            // Bardic disruption to silence foes
            SetSkill(SkillName.Musicianship, 100.0);
            SetSkill(SkillName.Discordance,   90.0);

            Fame           = 25000;
            Karma          = -25000;
            VirtualArmor  = 90;
            ControlSlots  = 5;

            // Initial cooldowns
            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummon    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_NextCurse     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMirror    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 45));
            m_LastLocation  = this.Location;

            // Loot reagents & chance for rare pharaoh relic
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new BloomwakeWaltz());
        }

        public override bool AlwaysMurderer   => true;
        public override Poison PoisonImmune   => Poison.Lethal;
        public override bool ReacquireOnMovement => true;
        public override bool AcquireOnApproach    => true;
        public override int AcquireOnApproachRange => 8;

        // -- Movement Hazard: Quicksand --
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || Map == null || m.Map != this.Map)
                return;

            // Drop a quicksand patch 20% of moves
            if (Utility.RandomDouble() < 0.20 && this.Location != m_LastLocation)
            {
                var qs = new QuicksandTile {
                    Hue = UniqueHue
                };

                var dropLoc = m_LastLocation;
                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                qs.MoveToWorld(dropLoc, this.Map);
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            // Priority: Sandstorm > Summon > Curse > Mirror
            if (now >= m_NextSandstorm && InRange(Combatant.Location, 10))
            {
                DoSandstorm();
                m_NextSandstorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextSummon)
            {
                SummonMummies();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (now >= m_NextCurse && InRange(Combatant.Location, 8))
            {
                ApplyWitheringCurse();
                m_NextCurse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (now >= m_NextMirror)
            {
                CastMirrorImages();
                m_NextMirror = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
            }
        }

        private void DoSandstorm()
        {
            Say("*Feel the wrath of the sands!*");
            PlaySound(0x5C5); // Gust sound

            // AoE wind/sand effect
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 8, 30, UniqueHue, 0, 5022, 0);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(25, 45);
                AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);

                if (m is Mobile target)
                {
                    target.SendMessage(0x22, "The raging sand slows your movements!");
                    target.Stam = Math.Max(0, target.Stam - Utility.RandomMinMax(20, 40));
                }
            }
        }

        private void SummonMummies()
        {
            Say("*Arise, guardians of the pharaoh!*");
            PlaySound(0x213); // Summon sound

            int count = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < count; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mummy = new Mummy();
                mummy.Hue = UniqueHue;
                mummy.MoveToWorld(loc, Map);
                mummy.Combatant = Combatant;
            }
        }

        private void ApplyWitheringCurse()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Wither beneath the desert's curse!*");
                PlaySound(0x482); // Curse sound

                DoHarmful(target);
                target.ApplyPoison(this, Poison.Deadly);
                target.SendMessage(0x22, "You feel your flesh decay!");
                target.FixedParticles(0x3709, 10, 15, 5012, UniqueHue, 0, EffectLayer.Head);
            }
        }

        private void CastMirrorImages()
        {
            Say("*Illusions protect me!*");
            PlaySound(0x1F1); // Mirror image sound
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Say("*The sands reclaim you!*");
            PlaySound(0x307); // Large explosion

            // Scatter flame-strike hazards
            for (int i = 0; i < Utility.RandomMinMax(4, 6); i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-4, 4),
                    Y + Utility.RandomMinMax(-4, 4),
                    Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new FlamestrikeHazardTile {
                    Hue = UniqueHue
                };
                tile.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,       Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.10) // 10% chance
                PackItem(new SteelroseCorslet());
        }

        public PyramidRouser(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize timers
            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummon    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_NextCurse     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMirror    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 45));
            m_LastLocation  = this.Location;
        }
    }
}
