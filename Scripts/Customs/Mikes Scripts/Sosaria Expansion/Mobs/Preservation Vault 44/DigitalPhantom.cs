using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh;    // If you want to borrow Chain Lightning visuals
using Server.Targeting;        // For targeting helpers

namespace Server.Mobiles
{
    [CorpseName("a digital phantom corpse")]
    public class DigitalPhantom : BaseCreature
    {
        // Cooldown trackers
        private DateTime m_NextDataStorm;
        private DateTime m_NextCorruptionRift;
        private DateTime m_NextPhaseShift;
        private Point3D m_LastLocation;

        // Unique hue—bright cyan
        private const int PhantomHue = 1275;

        [Constructable]
        public DigitalPhantom()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "a Digital Phantom";
            Body = 199;            // same as Rai-Ju
            BaseSoundID = 0x346;   // same as Rai-Ju
            Hue = PhantomHue;

            // — Stats —
            SetStr(200, 250);
            SetDex(150, 200);
            SetInt(600, 700);

            SetHits(900, 1000);
            SetStam(150, 200);
            SetMana(800, 1000);

            SetDamage(10, 15);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   80, 90);

            // — Skills —
            SetSkill(SkillName.EvalInt,    120.1, 130.0);
            SetSkill(SkillName.Magery,     120.1, 130.0);
            SetSkill(SkillName.MagicResist,120.2, 140.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics,     90.1, 100.0);
            SetSkill(SkillName.Wrestling,   90.1, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextDataStorm      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorruptionRift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextPhaseShift     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_LastLocation       = this.Location;

            // Base loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
        }

        // — Glitch Aura on Movement — minor energy damage + mana sap
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || m.Map != this.Map || !m.InRange(this.Location, 2))
                return;

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Mana sap
                int manaDrain = Utility.RandomMinMax(5, 15);
                if (target.Mana >= manaDrain)
                {
                    target.Mana -= manaDrain;
                    target.SendMessage(0x482, "Your mind flickers as your mana is siphoned!");
                    target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }

                // Minor energy damage
                AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0,0,0,0,100);
            }
        }

        // — Thought loop to trigger special abilities —
        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Phase Shift (blink) if in melee range and off cooldown
            if (DateTime.UtcNow >= m_NextPhaseShift && this.InRange(Combatant.Location, 6))
            {
                PhaseShift();
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Data Storm AoE
            else if (DateTime.UtcNow >= m_NextDataStorm && this.InRange(Combatant.Location, 8))
            {
                DataStorm();
                m_NextDataStorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Corruption Rift (hazard on enemy location)
            else if (DateTime.UtcNow >= m_NextCorruptionRift)
            {
                CorruptionRift();
                m_NextCorruptionRift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // — Data Storm: 6‑tile energy AoE + chance mana drain —
        private void DataStorm()
        {
            this.Say("*<<<SYSTEM OVERLOAD>>>*");
            this.PlaySound(0x20A);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(1.5)),
                0x376A, 20, 30, PhantomHue, 0, 9502, 0);

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && m is Mobile tgt && CanBeHarmful(tgt, false))
                    targets.Add(tgt);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(t, this, dmg, 0,0,0,0,100);

                // chance to sap more mana
                if (Utility.RandomDouble() < 0.4 && t is Mobile mt && mt.Mana >= 20)
                {
                    mt.Mana -= Utility.RandomMinMax(15,25);
                    mt.SendMessage(0x482, "A surge of data corrupts your focus!");
                    mt.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    mt.PlaySound(0x1F8);
                }
            }
        }

        // — Corruption Rift: drops a temporary digital‑poison hazard under the target —
        private void CorruptionRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Decrypting…*");
            PlaySound(0x22F);
            Point3D loc = target.Location;

            Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, TimeSpan.FromSeconds(0.5)),
                0x3728, 10, 10, PhantomHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                var tile = new PoisonTile();     // simulates “digital corruption”
                tile.Hue = PhantomHue;
                tile.MoveToWorld(loc, this.Map);
                Effects.PlaySound(loc, this.Map, 0x1F6);
            });
        }

        // — Phase Shift: blink behind your current target, leaving a glitch tile behind —
        private void PhaseShift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Point3D oldLoc = this.Location;
            Point3D dest = target.Location;

            // attempt to place 1 tile behind
            var offsets = new[] { new Point3D(-1,0,0), new Point3D(1,0,0), new Point3D(0,-1,0), new Point3D(0,1,0) };
            foreach (var off in offsets)
            {
                var tryLoc = new Point3D(dest.X + off.X, dest.Y + off.Y, dest.Z);
                if (Map.CanFit(tryLoc.X, tryLoc.Y, tryLoc.Z, 16, false, false))
                {
                    this.Location = tryLoc;
                    this.Map = Map;
                    break;
                }
            }

            this.Say("*<<PHASE SHIFT>>*");
            PlaySound(0x1FD);
            Effects.SendLocationParticles(EffectItem.Create(oldLoc, this.Map, TimeSpan.FromSeconds(0.75)),
                0x3709, 10, 15, PhantomHue, 0, 5052, 0);

            // leave a lingering glitch vortex at old spot
            var vortex = new VortexTile();
            vortex.Hue = PhantomHue;
            vortex.MoveToWorld(oldLoc, this.Map);
        }

        // — Death Explosion: glitch detonation + hazardous tiles —
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*<<SYSTEM FAILURE>>*");
                Effects.PlaySound(this.Location, this.Map, 0x211);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(1.5)),
                    0x3709, 20, 40, PhantomHue, 0, 5052, 0);

                // Scatter hazard tiles
                for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    // alternate between digital lightning storms and corruption
                    var tile = (i % 2 == 0) 
                        ? (Item)new LightningStormTile() 
                        : new PoisonTile();
                    tile.Hue = PhantomHue;
                    tile.MoveToWorld(loc, this.Map);

                    Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, TimeSpan.FromSeconds(1.0)),
                        0x376A, 10, 20, PhantomHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8,12));

            // 3% chance for a unique “Digital Core” artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new Dreadhowl());  // assume DigitalCore is defined elsewhere
        }

        public DigitalPhantom(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns after load
            m_NextDataStorm      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCorruptionRift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextPhaseShift     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
