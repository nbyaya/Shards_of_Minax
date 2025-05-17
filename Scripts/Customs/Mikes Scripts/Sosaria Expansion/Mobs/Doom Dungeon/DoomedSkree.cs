using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a doomed skree corpse")]
    public class DoomedSkree : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextDoomShriek;
        private DateTime m_NextWarpRift;
        private DateTime m_NextHellDive;
        private Point3D   m_LastLocation;

        // A deep blood‐red hue
        private const int DoomHue = 1175;

        [Constructable]
        public DoomedSkree()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "a doomed skree";
            Body           = 733;
            BaseSoundID    = 1585;
            Hue            = DoomHue;

            // ——— Stats ———
            SetStr( 500,  600);
            SetDex( 200,  250);
            SetInt( 350,  450);

            SetHits(2000, 2400);
            SetStam( 300,  350);
            SetMana( 500,  650);

            // ——— Damage & Resistances ———
            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire,     30);
            SetDamageType(ResistanceType.Energy,   40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     70, 80);
            SetResistance(ResistanceType.Cold,     40, 55);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   80, 90);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,      120.0, 140.0);
            SetSkill(SkillName.Magery,       120.0, 140.0);
            SetSkill(SkillName.MagicResist,  125.0, 145.0);
            SetSkill(SkillName.Meditation,   100.0, 110.0);
            SetSkill(SkillName.Tactics,       95.0, 105.0);
            SetSkill(SkillName.Wrestling,     95.0, 105.0);

            Fame          = 22000;
            Karma         = -22000;
            VirtualArmor  = 90;
            ControlSlots  = 5;

            // Seed initial cooldowns
            m_NextDoomShriek = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextWarpRift   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextHellDive   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new MaxxiaDust(Utility.RandomMinMax(5, 10)));      // Custom reagent
            PackItem(new CragstepBoots());                              // Thematic armor piece
            PackGold(2000, 3000);
        }

        // ——— Cursed Aura: burns and weakens those who circle too close ———
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            base.OnMovement(m, oldLoc);

            if (m != null && m != this && Alive && m.Alive && m.Map == this.Map && m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    // Fire‐burn
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 30, 0, 0, 70);
                    // Slow with cursed chill
                    target.SendLocalizedMessage(1070842, "", 0x22); // "A chilling aura surrounds you!"
                    target.Freeze( TimeSpan.FromSeconds(2));
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Spawn hellish floor hazards as it moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                // Try to drop a random tile at old location
                Item tile;
                switch (Utility.RandomMinMax(0, 3))
                {
                    case 0: tile = new PoisonTile(); break;
                    case 1: tile = new FlamestrikeHazardTile(); break;
                    default: tile = new VortexTile(); break;
                }

                tile.Hue = DoomHue;
                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                    tile.MoveToWorld(old, this.Map);
                else
                    tile.MoveToWorld(new Point3D(old.X, old.Y, Map.GetAverageZ(old.X, old.Y)), this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            var now = DateTime.UtcNow;

            // Doom Shriek: cone‐fear + damage
            if (now >= m_NextDoomShriek && InRange(Combatant.Location, 8))
            {
                DoomShriek();
                m_NextDoomShriek = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Warp Rift: teleport target then slam
            else if (now >= m_NextWarpRift)
            {
                WarpRift();
                m_NextWarpRift = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Hell Dive: leap to target, AoE stomp
            else if (now >= m_NextHellDive && InRange(Combatant.Location, 12))
            {
                HellDive();
                m_NextHellDive = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
        }

        // ——— Doom Shriek: cone of fear + fire/energy damage ———
        private void DoomShriek()
        {
            if (!(Combatant is Mobile)) return;

            Say("*SCREE-OOOM!*");
            PlaySound(1582);
            FixedParticles(0x376A, 10, 60, 5039, DoomHue, 0, EffectLayer.Waist);

            foreach (var m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile tgt && this.InLOS(tgt))
                {
                    DoHarmful(tgt);
                    // Mixed fire+energy
                    AOS.Damage(tgt, this, Utility.RandomMinMax(25, 40), 0, 50, 0, 0, 50);
                    // Fear effect
                    tgt.Stam -= Utility.RandomMinMax(10, 20);
                    tgt.SendLocalizedMessage(1070843, "", 0x22); // "You are overcome with terror!"
                }
            }
        }

        // ——— Warp Rift: teleport the Combatant beneath you and slam ———
        private void WarpRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Reality rends!*");
            PlaySound(1585);
            // Rift in under them
            Point3D dest = this.Location;
            target.SendMessage("The darkness swallows you!");
            target.Location = dest;
            target.ProcessDelta();

            // Slam AoE
            FixedParticles(0x3709, 10, 30, 5052, DoomHue, 0, EffectLayer.Waist);
            foreach (var m in Map.GetMobilesInRange(dest, 3))
            {
                if (m != this && m is Mobile tgt2 && CanBeHarmful(tgt2, false))
                {
                    DoHarmful(tgt2);
                    AOS.Damage(tgt2, this, Utility.RandomMinMax(30, 50), 50, 0, 0, 0, 50);
                }
            }
        }

        // ——— Hell Dive: leap to the target and cause a shockwave ———
        private void HellDive()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            PlaySound(1584);
            FixedParticles(0x3779, 10, 25, 9510, DoomHue, 0, EffectLayer.Head);
            // Jump visually—this is conceptual; actual pathing may vary
            target.Location = this.Location;  
            target.ProcessDelta();

            // Stomp
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration),
                0x36BD, 20, 10, DoomHue, 0, 5044, 0);

            foreach (var m in Map.GetMobilesInRange(Location, 4))
            {
                if (m != this && m is Mobile tgt3 && CanBeHarmful(tgt3, false))
                {
                    DoHarmful(tgt3);
                    AOS.Damage(tgt3, this, Utility.RandomMinMax(35, 60), 0, 0, 100, 0, 0);
                }
            }
        }

        // ——— Death Burst: spawn multiple hazards around the corpse ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*SCREEE-CRACK!*");
            PlaySound(1583);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 60, DoomHue, 0, 5052, 0);

            // Scatter 5–8 hazardous tiles
            int count = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                Item tile;
                switch (Utility.RandomMinMax(0, 2))
                {
                    case 0: tile = new NecromanticFlamestrikeTile(); break;
                    case 1: tile = new LightningStormTile();           break;
                    default: tile = new QuicksandTile();              break;
                }

                tile.Hue = DoomHue;
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // ——— Loot & Overrides ———
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new EchoveilRobe()); // 5% chance for a unique crafting reagent
        }

        public override bool BleedImmune  => true;
        public override int  TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public DoomedSkree(Serial serial) : base(serial) { }
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
            m_NextDoomShriek = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextWarpRift   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextHellDive   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
