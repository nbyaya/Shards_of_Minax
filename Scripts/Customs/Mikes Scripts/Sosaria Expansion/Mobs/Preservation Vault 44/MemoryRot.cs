using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a rotted mind corpse")]
    public class MemoryRot : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextAmnesiaTime;
        private DateTime m_NextSpikeTime;
        private DateTime m_NextFractureTime;
        private Point3D m_LastLocation;

        // Unique decayed-green hue
        private const int UniqueHue = 1302;

        [Constructable]
        public MemoryRot() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name               = "a memory rot";
            Body               = 319;   // reusing maggot mound body
            BaseSoundID        = 898;   // maggoty squelch
            Hue                = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(400, 450);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison,   70);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.EvalInt,      100.0, 110.0);
            SetSkill(SkillName.Magery,       100.0, 110.0);
            SetSkill(SkillName.MagicResist,  120.0, 130.0);
            SetSkill(SkillName.Meditation,   90.0,  100.0);
            SetSkill(SkillName.Tactics,      90.0,  100.0);
            SetSkill(SkillName.Wrestling,    90.0,  100.0);

            Fame               = 25000;
            Karma              = -25000;

            VirtualArmor       = 75;
            ControlSlots       = 5;

            // Initialize cooldowns
            m_NextAmnesiaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSpikeTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextFractureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation     = this.Location;

            // Base loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(10, 15)));
        }

        // Psychic Rot Aura: drains a bit of mana/stam and damages when players move too close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == null || !Alive || !m.Alive || m.Map != this.Map || !m.InRange(this.Location, 2) || !CanBeHarmful(m, false))
                return;

            if (m is Mobile target)
            {
                DoHarmful(target);

                // Drain mana or stamina
                if (target.Mana > 0 && Utility.RandomDouble() < 0.5)
                {
                    int drained = Utility.RandomMinMax(5, 15);
                    target.Mana = Math.Max(0, target.Mana - drained);
                    target.SendMessage("Your mind feels clouded and your magic wanes!");
                }
                else
                {
                    int drained = Utility.RandomMinMax(5, 15);
                    target.Stam = Math.Max(0, target.Stam - drained);
                    target.SendMessage("A wave of disorientation saps your strength!");
                }

                // Minor poison damage
                AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                target.PlaySound(0x20A);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave memory-rot sludge tiles behind occasionally
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    PoisonTile sludge = new PoisonTile();
                    sludge.Hue = UniqueHue;
                    sludge.MoveToWorld(old, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || Map == null || !Alive)
                return;

            // Amnesia Wave: AoE confusion
            if (DateTime.UtcNow >= m_NextAmnesiaTime && this.InRange(Combatant.Location, 8))
            {
                AmnesiaWave();
                m_NextAmnesiaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Neural Spike: direct strike + mana burn
            else if (DateTime.UtcNow >= m_NextSpikeTime && this.InRange(Combatant.Location, 12))
            {
                NeuralSpike();
                m_NextSpikeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Mind Fracture: spawns illusionary copies
            else if (DateTime.UtcNow >= m_NextFractureTime)
            {
                MindFracture();
                m_NextFractureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Amnesia Wave ---
        public void AmnesiaWave()
        {
            this.Say("*Your memories twist!*");
            PlaySound(0x228);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x374A, 12, 20, UniqueHue, 0, 5032, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && m is Mobile)
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                DoHarmful(t);
                t.Paralyzed = true;
                Timer.DelayCall(TimeSpan.FromSeconds(2.0), () => { if (t != null && !t.Deleted) t.Paralyzed = false; });
                t.SendMessage("Memories slip away as your mind shatters!");
            }
        }

        // --- Neural Spike ---
        public void NeuralSpike()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Feel the torment of forgotten thought!*");
            PlaySound(0x1F9);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4, 10, 5, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

            DoHarmful(target);

            int dmg = Utility.RandomMinMax(50, 75);
            AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);

            // Burn some mana
            if (target.Mana >= 30)
            {
                target.Mana -= 30;
                target.SendMessage("A searing pain burns away your magical reserves!");
            }
        }

        // --- Mind Fracture ---
        public void MindFracture()
        {
            this.Say("*Reflections of despair!*");
            PlaySound(0x22C);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(
                    this.X + Utility.RandomList(-2, -1, 1, 2),
                    this.Y + Utility.RandomList(-2, -1, 1, 2),
                    this.Z
                );

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var illusion = new MoundOfMaggots(); // reuses maggot mound as illusion
                illusion.Hue = UniqueHue;
                illusion.MoveToWorld(loc, this.Map);

                // Illusion despawns quickly
                Timer.DelayCall(TimeSpan.FromSeconds(8.0), () =>
                {
                    if (illusion != null && !illusion.Deleted)
                        illusion.Delete();
                });
            }
        }

        // --- Death Effect ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            this.Say("*Memories... reclaimed...*");
            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 40, UniqueHue, 0, 5052, 0);

            // Spawn toxic gas and poison hazards around corpse
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                ToxicGasTile gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(new Point3D(x, y, z), Map);

                PoisonTile slime = new PoisonTile();
                slime.Hue = UniqueHue;
                slime.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // Properties & Loot Overrides
        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,         Utility.RandomMinMax(10, 14));
            AddLoot(LootPack.MedScrolls);
            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new TheMoonherder()); // custom reagent
        }

        public MemoryRot(Serial serial) : base(serial) { }

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
            m_NextAmnesiaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSpikeTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextFractureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation     = this.Location;
        }
    }
}
