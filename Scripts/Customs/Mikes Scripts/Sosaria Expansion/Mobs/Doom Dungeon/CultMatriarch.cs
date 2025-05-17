using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cult matriarch corpse")]
    public class CultMatriarch : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextAuraTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextCurseTime;

        // Last known position for trailing effect
        private Point3D m_LastLocation;

        // A sickly, eldritch green
        private const int UniqueHue = 1175;

        [Constructable]
        public CultMatriarch()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name           = "a cult matriarch";
            Body           = 87;     // same as OphidianMatriarch
            BaseSoundID    = 644;    // same as OphidianMatriarch
            Hue            = UniqueHue;

            // — Stats —
            SetStr(500, 600);
            SetDex(120, 140);
            SetInt(700, 800);

            SetHits(1800, 2100);
            SetStam(150, 180);
            SetMana(1000, 1200);

            SetDamage(20, 25);

            // — Damage Types (mostly magic) —
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire,     20);
            SetDamageType(ResistanceType.Cold,     20);
            SetDamageType(ResistanceType.Poison,   20);
            SetDamageType(ResistanceType.Energy,   30);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     55, 65);
            SetResistance(ResistanceType.Cold,     55, 65);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   80, 90);

            // — Skills —
            SetSkill(SkillName.EvalInt,    120.0, 130.0);
            SetSkill(SkillName.Magery,     120.0, 130.0);
            SetSkill(SkillName.MagicResist,130.0, 140.0);
            SetSkill(SkillName.Meditation,100.0, 110.0);
            SetSkill(SkillName.Tactics,     95.0, 100.0);
            SetSkill(SkillName.Wrestling,   95.0, 100.0);

            Fame           = 25000;
            Karma          = -25000;
            VirtualArmor  = 90;
            ControlSlots   = 5;

            // Initialize cooldowns
            m_NextAuraTime   = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextCurseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(12);

            m_LastLocation = this.Location;
        }

        // — Aura: Sinister Corruption —
        // Applies damage & a debuff tile whenever someone moves within 3 tiles
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.UtcNow >= m_NextAuraTime && m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    target.SendMessage(0x22, "A wave of corrupt energy seeps into your bones!"); 
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);

                    // Spawn a toxic gas patch under them
                    var gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(target.Location, this.Map);

                    m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(8);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // — Main AI Loop for special abilities & trailing effect —
        public override void OnThink()
        {
            base.OnThink();

            // Leave behind a fleeting trap web where she steps
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var web = new TrapWeb();
                web.Hue = UniqueHue;
                web.MoveToWorld(m_LastLocation, this.Map);
            }
            m_LastLocation = this.Location;

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Summon Acolytes
            if (DateTime.UtcNow >= m_NextSummonTime && this.InRange(Combatant.Location, 15))
            {
                SummonAcolytes();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
            // Blood Curse: a direct hex
            else if (DateTime.UtcNow >= m_NextCurseTime && this.InRange(Combatant.Location, 10))
            {
                BloodCurse();
                m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(18);
            }
        }

        // — Ability #1: Summon two lesser cultists around her —
        public void SummonAcolytes()
        {
            this.Say("*Rise, my children!*");
            PlaySound(0x1F2);
            for (int i = 0; i < 2; i++)
            {
                Point3D spawn = new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z);
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                // “CultAcolyte” should be a smaller BaseCreature you define elsewhere
                var acolyte = new MeerMage();
                acolyte.Hue = UniqueHue;
				acolyte.Name = "Cult Acolyte";
                acolyte.MoveToWorld(spawn, this.Map);
            }
        }

        // — Ability #2: Blood Curse — targeted AoE poison + stam/health drain —
        public void BloodCurse()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*Taste the rot of your own blood!*");
                PlaySound(0x228);
                // Center effect
                Effects.SendLocationParticles(
                    EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 8, 20, UniqueHue, 0, 5042, 0);

                // Direct damage + poison
                AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);
                target.ApplyPoison(this, Poison.Deadly);

                // Drain stamina
                int stamDrain = Utility.RandomMinMax(20, 40);
                if (target.Stam >= stamDrain)
                {
                    target.Stam -= stamDrain;
                    target.SendMessage(0x22, "You feel your strength ebb away!");
                }

                // Spawn short‐lived rot tile
                var rot = new PoisonTile();
                rot.Hue = UniqueHue;
                rot.MoveToWorld(target.Location, this.Map);
            }
        }

        // — On death, unleash a final wave of cultic ruin —
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The mother returns to ash…*");
                PlaySound(0x1E5);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x376A, 12, 30, UniqueHue, 0, 5039, 0);

                // Scatter several Necromantic Flamestrike hazards around
                for (int i = 0; i < 5; i++)
                {
                    var fx = new NecromanticFlamestrikeTile();
                    int xOff = Utility.RandomMinMax(-3, 3), yOff = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(this.X + xOff, this.Y + yOff, this.Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                    fx.Hue = UniqueHue;
                    fx.MoveToWorld(loc, this.Map);
                }
            }

            base.OnDeath(c);
        }

        // — Loot & basics —
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls, 3);

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new FortressBornShinplates()); // (placeholder unique drop)
        }

        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public CultMatriarch(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on load
            m_NextAuraTime   = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextCurseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }
    }
}
