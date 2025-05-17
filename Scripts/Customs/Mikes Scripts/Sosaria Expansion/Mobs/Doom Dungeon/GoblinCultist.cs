using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh;   // for Chain Lightning template
using Server.Spells.Ninjitsu;  // for Poison
using Server.Spells.Sixth;     // for Paralyze Field

namespace Server.Mobiles
{
    [CorpseName("a goblin cultist corpse")]
    public class GoblinCultist : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextSummonTime;
        private DateTime m_NextHexTime;
        private DateTime m_NextRitualTime;
        private Point3D m_LastLocation;

        // Unique hue for this boss
        private const int CultHue = 2100;  

        [Constructable]
        public GoblinCultist() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a goblin cultist";
            Body = 723;
            Hue = CultHue;
            BaseSoundID = 0x600;

            // Enhanced stats
            SetStr(300, 350);
            SetDex(100, 150);
            SetInt(400, 450);

            SetHits(600, 700);
            SetStam(100, 150);
            SetMana(400, 500);

            SetDamage(10, 15);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison, 70);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 115.0);
            SetSkill(SkillName.MagicResist, 110.0, 125.0);
            SetSkill(SkillName.Poisoning, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 95.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextHexTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextRitualTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new ScavengersWarding());
            PackGold(500, 800);
            PackItem(new ThighBoots());

            // Reagents for dark rituals
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // Poisonous aura: hurts and poisons anyone moving too close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m == null || !Alive || m.Map != this.Map || !m.InRange(this.Location, 2))
                return;

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);
                target.ApplyPoison(this, Poison.Deadly);
                target.FixedParticles(0x374A, 10, 15, 5032, CultHue, 0, EffectLayer.Head);
                target.PlaySound(0x1F9);
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Summon goblin warriors
            if (DateTime.UtcNow >= m_NextSummonTime && this.InRange(Combatant.Location, 12))
            {
                SummonGoblinPack();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            // Cast a crippling hex
            else if (DateTime.UtcNow >= m_NextHexTime && this.InRange(Combatant.Location, 10))
            {
                CastCripplingHex();
                m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Dark ritual: leech life & mana
            else if (DateTime.UtcNow >= m_NextRitualTime && this.InRange(Combatant.Location, 8))
            {
                PerformRitualDrain();
                m_NextRitualTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 45));
            }

            // Leave trapweb occasionally as it moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                var tile = new TrapWeb();
                tile.Hue = CultHue;
                if (!Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                    old.Z = Map.GetAverageZ(old.X, old.Y);
                tile.MoveToWorld(old, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        private void SummonGoblinPack()
        {
            Say("*Rise, my minions!*");
            Effects.PlaySound(Location, Map, 0x1FE);
            for (int i = 0; i < 3; i++)
            {
                var gob = new GrayGoblinMage();
                gob.Location = new Point3D(
                    this.X + Utility.RandomList(-1, 1),
                    this.Y + Utility.RandomList(-1, 1),
                    this.Z
                );
                gob.Hue = CultHue;
                gob.Team = this.Team;
                gob.MoveToWorld(gob.Location, Map);
                gob.Combatant = this.Combatant;
            }
        }

        private void CastCripplingHex()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*Feel the curse of the cult!*");
            PlaySound(0x212);

            // Properly construct and cast Paralyze Field
            var spell = new ParalyzeFieldSpell(this, null);
            spell.Cast();
        }

        private void PerformRitualDrain()
        {
            if (!(Combatant is Mobile target)) return;
            Say("*Your essence belongs to the cult!*");
            PlaySound(0x210);

            // Drain life
            int lifeDrain = Utility.RandomMinMax(30, 50);
            AOS.Damage(target, this, lifeDrain, 0, 0, 0, 0, 100);

            // Drain mana & stamina
            int manaDrain = Utility.RandomMinMax(20, 40);
            int stamDrain = Utility.RandomMinMax(20, 40);
            if (target.Mana >= manaDrain)
            {
                target.Mana -= manaDrain;
                target.SendMessage(0x22, "You feel your magical energy siphoned away!");
            }
            if (target.Stam >= stamDrain)
            {
                target.Stam -= stamDrain;
                target.SendMessage(0x44, "Your strength ebbs away under the cult's ritual!");
            }

            // Create a PoisonTile at their feet
            var pTile = new PoisonTile();
            pTile.Hue = CultHue;
            var loc = target.Location;
            if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);
            pTile.MoveToWorld(loc, Map);

            target.FixedParticles(0x376A, 10, 15, 5032, CultHue, 0, EffectLayer.Waist);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            
			if (Map == null) return;
			
			// Spawn a ring of landmines around corpse
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-2, 2);
                int dy = Utility.RandomMinMax(-2, 2);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var mine = new LandmineTile();
                mine.Hue = CultHue;
                mine.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new FangwardensMaw()); // unique drop
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus    => 60.0;

        public GoblinCultist(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers so a restart doesn't stall abilities
            var now = DateTime.UtcNow;
            m_NextSummonTime = now + TimeSpan.FromSeconds(20);
            m_NextHexTime    = now + TimeSpan.FromSeconds(12);
            m_NextRitualTime = now + TimeSpan.FromSeconds(25);
        }
    }
}
