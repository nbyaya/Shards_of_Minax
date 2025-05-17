using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.First; // For Magic Arrow effect
using Server.Spells.Third; // For Bless/Curse effects potentially
using Server.Spells.Fourth; // For Curse potentially
using Server.Spells.Sixth; // For EnergyBoltSpell (used in place of ManaDrainSpell)
using Server.Spells.Seventh; // For MeteorSwarmSpell, ChainLightningSpell
using Server.Spells.Fifth;  // For MindBlastSpell
using Server.Network;      // Needed for effects

namespace Server.Mobiles
{
    [CorpseName("a pulsating magical remnant")] // More thematic corpse name
    public class Hexling : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextManaPulseTime;
        private DateTime m_NextHexTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextPhaseTime;

        // Unique Hue - Example: 1266 is a vibrant shifting purple/blue.
        public const int UniqueHue = 1266;
        private bool m_IsInPhase = false; // Track phase state

        [Constructable]
        public Hexling() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Agile AI
        {
            Name = "a Hexling";
            Body = 153; // Bogle body
            BaseSoundID = 0x482; // Bogle sound
            Hue = UniqueHue;

            // --- Significantly Boosted Stats - Magic Focused ---
            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(550, 650); // High Intelligence for magic power

            SetHits(1100, 1400); // High health pool
            SetStam(150, 200);
            SetMana(1000, 1500); // Very Large Mana pool

            SetDamage(18, 24); // Moderate base damage

            // Primarily Energy/Magic damage
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 70);

            // --- Adjusted Resistances - Strong vs Magic ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 55, 65);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 65, 75); // Resistant to decay/poison magics
            SetResistance(ResistanceType.Energy, 80, 90); // Very high energy/magic resist

            // --- Enhanced Skills - Magic Focus ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 120.2, 135.0); // Top-tier magic resist
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);
            SetSkill(SkillName.SpiritSpeak, 85.0, 95.0); // Fits the ghostly/academy theme

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80; // High passive defense
            ControlSlots = 5; // Difficult to control

            // Initialize ability cooldowns (staggered start)
            m_NextManaPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            m_NextPhaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            // Pack some thematic reagents
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));

            // Optional: Add a light source if fitting (like the bogle)
            // AddItem( new LightSource() );
        }

        // --- Thinking Process & Ability Execution ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Check Phase Shift Timer
            if (!m_IsInPhase && DateTime.UtcNow >= m_NextPhaseTime)
            {
                ActivatePhaseShift();
                // Reset cooldown AFTER activation
                m_NextPhaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }

            // Check other abilities, maybe less frequent if phased? (Optional complexity)
            // Or prioritize based on target's state (e.g., hex if target mana high)

            // Prioritize abilities based on cooldowns and range
            if (DateTime.UtcNow >= m_NextRiftTime && this.InRange(Combatant.Location, 10))
            {
                CastRiftCollapse();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextHexTime && this.InRange(Combatant.Location, 8))
            {
                CastUnstableHex();
                m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (DateTime.UtcNow >= m_NextManaPulseTime && this.InRange(Combatant.Location, 6))
            {
                CastManaSiphonPulse();
                m_NextManaPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Basic spell casting if abilities on cooldown or out of range
            else if (AIObject.Action == ActionType.Combat && Combatant != null && !this.InRange(Combatant.Location, 2) && Utility.RandomDouble() < 0.3) // 30% chance to cast if not using ability and at range
            {
                // Cast high-level damaging or debuffing spells
                if (Combatant is Mobile mobileTarget && mobileTarget.Mana > 20 && Utility.RandomBool())
                {
                    // Replacing ManaDrainSpell with EnergyBoltSpell since ManaDrainSpell is unavailable
                    Spell spell = new Server.Spells.Sixth.EnergyBoltSpell(this, null);
                    spell.Cast();
                }
                else
                {
                    switch(Utility.Random(4))
                    {
                        case 0: Spell spell0 = new Server.Spells.Seventh.MeteorSwarmSpell(this, null); spell0.Cast(); break;
                        case 1: Spell spell1 = new Server.Spells.Seventh.ChainLightningSpell(this, null); spell1.Cast(); break;
                        case 2: Spell spell2 = new Server.Spells.Sixth.EnergyBoltSpell(this, null); spell2.Cast(); break;
                        case 3: Spell spell3 = new Server.Spells.Fifth.MindBlastSpell(this, null); spell3.Cast(); break;
                    }
                }
            }
        }

        // --- Unique Ability 1: Mana Siphon Pulse ---
        public void CastManaSiphonPulse()
        {
            if (Map == null || !Alive) return;

            this.PlaySound(0x1F8); // Energy Bolt sound or Mana Drain sound (0x1F9)
            // Visual effect: Expanding purple ring from self
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3789, 10, 20, UniqueHue, 0, 5032, 0); // Purple/blue energy field effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius AoE

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                this.Say("*Your magic is mine!*"); // Optional flavor text

                foreach (Mobile target in targets)
                {
                    // Safety check: Ensure target is a Mobile before accessing Mana/Stam
                    if (target is Mobile mobileTarget)
                    {
                        DoHarmful(target);
                        int manaDrained = Utility.RandomMinMax(30, 50);
                        int stamDrained = Utility.RandomMinMax(20, 40);
                        int damage = manaDrained / 3; // Deal some damage based on mana drained

                        // Drain Mana
                        if (mobileTarget.Mana >= manaDrained)
                        {
                            mobileTarget.Mana -= manaDrained;
                            this.Mana += manaDrained; // Hexling gains the mana
                        }
                        else
                        {
                            damage += (manaDrained - mobileTarget.Mana); // Extra damage if not enough mana
                            this.Mana += mobileTarget.Mana;
                            mobileTarget.Mana = 0;
                        }

                        // Drain Stamina
                        if (mobileTarget.Stam >= stamDrained)
                        {
                            mobileTarget.Stam -= stamDrained;
                        }
                        else
                        {
                            mobileTarget.Stam = 0;
                        }

                        // Apply Damage (Energy type)
                        AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                        // Visual effect on target (casting EffectLayer to int)
                        target.FixedParticles(0x374A, 10, 25, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x1F9); // Mana drain sound on target
                        mobileTarget.SendMessage("Your spiritual energies are violently drained!");
                    }
                }
            }
        }

        // --- Unique Ability 2: Unstable Hex ---
        public void CastUnstableHex()
        {
            if (Combatant == null || Map == null || !Alive) return;

            // Safety check: Ensure Combatant is a Mobile
            if (Combatant is Mobile target)
            {
                if (!CanBeHarmful(target)) return;

                DoHarmful(target);
                this.Say("*Suffer my curse!*");
                target.PlaySound(0x1E6); // Curse sound
                target.FixedParticles(0x374A, 10, 15, 5028, UniqueHue, 0, EffectLayer.Waist); // Dark curse effect

                int choice = Utility.Random(5); // Choose a random hex

                switch(choice)
                {
                    case 0: // Weaken
                        target.AddStatMod(new StatMod(StatType.Str, "HexWeaken", -Utility.RandomMinMax(15, 30), TimeSpan.FromSeconds(20)));
                        target.SendMessage("You feel drastically weakened!");
                        break;
                    case 1: // Clumsy
                        target.AddStatMod(new StatMod(StatType.Dex, "HexClumsy", -Utility.RandomMinMax(15, 30), TimeSpan.FromSeconds(20)));
                        target.SendMessage("You stumble awkwardly!");
                        break;
                    case 2: // Feeblemind
                        target.AddStatMod(new StatMod(StatType.Int, "HexFeeble", -Utility.RandomMinMax(15, 30), TimeSpan.FromSeconds(20)));
                        target.SendMessage("Your thoughts become muddled!");
                        break;
                    case 3: // Standard Curse
                        SpellHelper.AddStatCurse(this, target, StatType.Str | StatType.Dex | StatType.Int);
                        target.SendMessage("A potent magical curse afflicts you!");
                        break;
                    case 4: // Magic Resistance Debuff
                        // Lower resist temporarily (if not supported, just apply damage)
                        AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                        target.SendMessage("Arcane energies burn within you!");
                        break;
                }
            }
        }

        // --- Unique Ability 3: Rift Collapse ---
        public void CastRiftCollapse()
        {
            if (Combatant == null || Map == null || !Alive) return;

            // Safety check: Ensure Combatant is a Mobile to get Location
            if (Combatant is Mobile target)
            {
                Point3D targetLocation = target.Location;

                if (!CanBeHarmful(target)) return;

                DoHarmful(target); // Mark as harmful action towards the intended target area
                this.Say("*The veil thins!*");
                PlaySound(0x228); // Teleport sound or Gate Travel sound (0x1FC)

                // Visual effect: Rift opening at target location (cast layer to int)
                Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5032, (int)EffectLayer.Waist);

                // Delay spawn of the damaging tile slightly
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (this.Map == null || !this.Alive) return; // Check validity again

                    Point3D spawnLoc = targetLocation;
                    // Validate spawn location on map
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    {
                        spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                        if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                            return; // Give up if no valid spot found near target
                    }

                    // Spawn a Vortex Tile or Chaotic Teleport Tile
                    VortexTile tile = new VortexTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(spawnLoc, this.Map);

                    // Optional: Hexling teleports away slightly after casting
                    if(Utility.RandomBool())
                    {
                        Point3D newLoc = this.Location;
                        for (int i = 0; i < 5; ++i) // Try 5 times to find a spot
                        {
                            Point3D tryLoc = new Point3D(this.X + Utility.RandomMinMax(-5, 5), this.Y + Utility.RandomMinMax(-5, 5), this.Z);
                            if (Map.CanFit(tryLoc.X, tryLoc.Y, tryLoc.Z, this.Body.IsHuman ? 16 : 5, true, false))
                            {
                                newLoc = tryLoc;
                                break;
                            }
                        }
                        // Teleport effect on self
                        Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0);
                        this.Location = newLoc;
                        Effects.SendLocationParticles(EffectItem.Create(newLoc, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0);
                        this.PlaySound(0x1FE); // Teleport sound
                    }
                });
            }
        }

        // --- Unique Ability 4: Phase Shift ---
		public void ActivatePhaseShift()
		{
			if (Map == null || !Alive || m_IsInPhase)
				return;

			m_IsInPhase = true;
			this.Say("*You cannot grasp me!*");
			this.PlaySound(0x5C8); // Phasing sound effect

			// Visual: Apply ethereal transparency or shimmer
			this.FixedParticles(0x376A, 9, 32, 5032, UniqueHue, 0, EffectLayer.Waist); // Ethereal effect

			int bonus = 20; // Increase resistances by 20
			TimeSpan duration = TimeSpan.FromSeconds(10);

			List<ResistanceMod> tempResistMods = new List<ResistanceMod>
			{
				new ResistanceMod(ResistanceType.Physical, bonus),
				new ResistanceMod(ResistanceType.Fire, bonus),
				new ResistanceMod(ResistanceType.Cold, bonus),
				new ResistanceMod(ResistanceType.Poison, bonus),
				new ResistanceMod(ResistanceType.Energy, bonus)
			};

			foreach (var mod in tempResistMods)
				this.AddResistanceMod(mod);

			// Remove bonuses after duration
			Timer.DelayCall(duration, () =>
			{
				if (!this.Deleted && this.Alive)
				{
					foreach (var mod in tempResistMods)
						this.RemoveResistanceMod(mod);
				}
			});

			// Timer to end the phase shift
			Timer.DelayCall(duration, () =>
			{
				if (!this.Deleted && this.Alive)
				{
					m_IsInPhase = false;
					this.PlaySound(0x5C9); // Phase out sound
					this.SendMessageIfCanSee("*The Hexling solidifies!*"); // Notify nearby players
				}
			});
		}


        // Helper method to send a message if nearby mobiles can see the Hexling.
        private void SendMessageIfCanSee(string message)
        {
            // For simplicity, this sends an overhead message.
            PublicOverheadMessage(MessageType.Regular, 0, false, message);
        }

        // --- Death Effect: Arcane Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            // Play a powerful sound
            Effects.PlaySound(this.Location, this.Map, 0x211); // Dispel/Energy explosion sound

            // Large central visual explosion
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(1.0)), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big purple explosion

            int tilesToDrop = Utility.RandomMinMax(6, 10); // Number of mana drain tiles
            List<Point3D> locations = new List<Point3D>();

            for (int i = 0; i < tilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D dropLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);
                    if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                        continue; // Skip if invalid
                }

                locations.Add(dropLoc);
            }

            // Delay spawning tiles slightly for main explosion visual
            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (this.Map == null) return; // Recheck map validity

                foreach (Point3D loc in locations)
                {
                    ManaDrainTile tile = new ManaDrainTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, this.Map);
                    // Smaller visual effect per tile
                    Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration), 0x374A, 5, 10, UniqueHue, 0, 5016, 0);
                }
            });

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Magical beings don't bleed typically
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to mundane poisons

        // Increased Dispel difficulty (as it's highly magical)
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2); // Very good base loot
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2)); // Chance for high scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // More gems

            // Chance for a unique thematic drop
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                PackItem(new MalidorsFragment()); // Assuming MalidorsFragment is defined elsewhere
            }
            if (Utility.RandomDouble() < 0.01) // 1% chance for another rare item
            {
                switch(Utility.Random(3))
                {
                    case 0: PackItem(new EarringsOfManaDrain()); break;
                    case 1: PackItem(new HexlingShroud()); break;
                    case 2: PackItem(new RingOfUnstableMagic()); break;
                }
            }
        }

        // --- Serialization ---
        public Hexling(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Serialize m_IsInPhase state
            writer.Write(m_IsInPhase);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize cooldowns on load/restart
            m_NextManaPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextHexTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            m_NextPhaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));

            m_IsInPhase = reader.ReadBool();
            if(m_IsInPhase)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => {
                    if (!this.Deleted && this.Alive)
                    {
                        m_IsInPhase = false;
                        // Additional visuals/sounds can be added here if desired.
                    }
                });
            }
        }
    }

    // --- Placeholder definitions for unique loot items ---
    // You would need to create these item files separately.

    public class MalidorsFragment : Item
    {
        [Constructable]
        public MalidorsFragment() : base(0x1F1C) // Example ItemID (piece of crystal/bone)
        {
            Name = "Malidor's Fragment";
            Hue = Hexling.UniqueHue; // Match the Hexling's hue
            Weight = 1.0;
            LootType = LootType.Regular;
            Stackable = false;
        }
        public MalidorsFragment(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class EarringsOfManaDrain : BaseJewel // Assuming BaseJewel exists
    {
        [Constructable]
        public EarringsOfManaDrain() : base(0x1087, Layer.Earrings) // Example ItemID for earrings
        {
            Name = "Earrings of Mana Siphon";
            Hue = Hexling.UniqueHue;
            Weight = 0.1;
            Resistances.Energy = 5;
            Attributes.BonusInt = 5;
            Attributes.RegenMana = 2;
        }
        public EarringsOfManaDrain(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class HexlingShroud : BaseCloak // Assuming BaseCloak exists
    {
        [Constructable]
        public HexlingShroud() : base(0x1515) // Example ItemID for cloak
        {
            Name = "Hexling Shroud";
            Hue = Hexling.UniqueHue;
            Weight = 3.0;
            Resistances.Energy = 10;
            Attributes.SpellDamage = 5;
            Attributes.LowerManaCost = 8;
        }
        public HexlingShroud(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class RingOfUnstableMagic : BaseRing // Assuming BaseRing exists
    {
        [Constructable]
        public RingOfUnstableMagic() : base(0x108A) // Example ItemID for ring
        {
            Name = "Ring of Unstable Magic";
            Hue = Hexling.UniqueHue;
            Weight = 0.1;
            Resistances.Energy = 7;
            Attributes.BonusMana = 10;
            Attributes.CastRecovery = 1;
        }
        public RingOfUnstableMagic(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}
